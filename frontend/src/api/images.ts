import apiClient from './config'
import type { Image, ImageGroup } from '../types'

export const imagesApi = {
  // 获取队列的所有图片
  getQueueImages: async (queueId: number): Promise<Image[]> => {
    const response = await apiClient.get<Image[]>(`/images/queue/${queueId}`)
    return response.data
  },

  // 获取下一组待选择的图片
  getNextGroup: async (queueId: number): Promise<ImageGroup | { message: string; completed: boolean }> => {
    const response = await apiClient.get(`/images/next-group/${queueId}`)
    return response.data
  },

  // 导入图片（批量上传 - 已弃用，建议使用并行单文件上传）
  importImages: async (
    queueId: number,
    files: File[],
    folderNames: string[],
    onProgress?: (progress: number) => void
  ): Promise<{ message: string }> => {
    const formData = new FormData()
    formData.append('queueId', queueId.toString())
    
    files.forEach((file) => {
      formData.append('files', file)
    })
    
    folderNames.forEach((name) => {
      formData.append('folderNames', name)
    })

    const response = await apiClient.post('/images/import', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
      timeout: 300000, // 5 分钟超时
      onUploadProgress: (progressEvent) => {
        if (onProgress && progressEvent.total) {
          const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total)
          onProgress(percentCompleted)
        }
      },
    })
    return response.data
  },

  // 单文件上传（并行上传使用）
  importSingleImage: async (
    queueId: number,
    file: File,
    folderName: string,
    onProgress?: (progress: number) => void
  ): Promise<{ message: string; imageId: number; fileName: string }> => {
    const formData = new FormData()
    formData.append('queueId', queueId.toString())
    formData.append('file', file)
    formData.append('folderName', folderName)

    const response = await apiClient.post('/images/import-single', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
      timeout: 300000, // 5 分钟超时
      onUploadProgress: (progressEvent) => {
        if (onProgress && progressEvent.total) {
          const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total)
          onProgress(percentCompleted)
        }
      },
    })
    return response.data
  },

  // 并行上传多张图片
  importImagesParallel: async (
    queueId: number,
    files: File[],
    folderNames: string[],
    onFileProgress?: (fileIndex: number, progress: number) => void,
    onOverallProgress?: (completed: number, total: number) => void,
    concurrency: number = 5
  ): Promise<{ message: string; successCount: number; failedCount: number; errors: string[] }> => {
    if (files.length !== folderNames.length) {
      throw new Error('文件和文件夹名称数量不匹配')
    }

    const total = files.length
    let completed = 0
    let successCount = 0
    let failedCount = 0
    const errors: string[] = []
    
    // 创建上传任务
    const uploadTasks = files.map((file, index) => ({
      file,
      folderName: folderNames[index],
      index
    }))

    // 并发控制：使用 Promise 队列限制并发数
    const executeWithLimit = async (tasks: typeof uploadTasks) => {
      const executing: Promise<void>[] = []
      
      for (const task of tasks) {
        // 创建promise并立即添加到executing数组
        let promiseRef: Promise<void> | null = null
        
        const promise = (async () => {
          try {
            await imagesApi.importSingleImage(
              queueId,
              task.file,
              task.folderName,
              (progress) => {
                if (onFileProgress) {
                  onFileProgress(task.index, progress)
                }
              }
            )
            successCount++
          } catch (error: any) {
            failedCount++
            const errorMsg = `${task.file.name}: ${error.response?.data?.message || error.message || '上传失败'}`
            errors.push(errorMsg)
            console.error(`文件 ${task.file.name} 上传失败:`, error)
          } finally {
            completed++
            if (onOverallProgress) {
              onOverallProgress(completed, total)
            }
            // 从executing数组中移除自己
            if (promiseRef) {
              const index = executing.indexOf(promiseRef)
              if (index > -1) {
                executing.splice(index, 1)
              }
            }
          }
        })()
        
        promiseRef = promise
        executing.push(promise)
        
        // 当达到并发限制时，等待任意一个完成
        if (executing.length >= concurrency) {
          await Promise.race(executing)
        }
      }
      
      // 等待所有剩余任务完成
      await Promise.all(executing)
    }

    await executeWithLimit(uploadTasks)

    return {
      message: `上传完成：成功 ${successCount} 个，失败 ${failedCount} 个`,
      successCount,
      failedCount,
      errors
    }
  },

  // 删除图片
  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/images/${id}`)
  },
}

