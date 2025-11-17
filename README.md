# 图片选取对比系统

一个完整的前后端分离的图片选取对比系统，支持多项目、多队列管理，用户可以从多个图片中选择最佳图片，并记录选择进度。

## 技术栈

### 后端
- ASP.NET Core 8.0
- Entity Framework Core
- MySQL 数据库
- JWT 认证
- Pomelo MySQL 驱动

### 前端
- Vue 3 + TypeScript
- Vite
- Element Plus UI
- Pinia 状态管理
- Vue Router
- Axios

## 功能特性

### 用户功能
- ✅ 用户注册和登录
- ✅ 浏览项目列表
- ✅ 查看项目的队列
- ✅ 图片对比选择（支持2-10张同时对比）
- ✅ 实时显示选择进度
- ✅ 自动保存选择记录

### 管理员功能
- ✅ 项目管理（创建、编辑、删除）
- ✅ 队列管理（创建、编辑、删除、配置对比数量）
- ✅ 批量导入图片（从多个同名文件夹）
- ✅ 导出选择数据（CSV/JSON）
- ✅ 查看所有用户进度

## 数据库配置

系统使用 MySQL 数据库，连接信息已配置：
- 服务器: `100.72.46.27`
- 数据库: `dotnet`
- 用户名: `dotnet`
- 密码: `dotnet`

数据库将在首次运行时自动创建表结构。

## 快速开始

### 1. 运行后端

```bash
cd Backend
dotnet restore
dotnet run
```

后端将在 `http://localhost:5000` 启动。

API 文档（Swagger）：`http://localhost:5000/swagger`

### 2. 运行前端

```bash
# 确保使用 Node.js 24
nvs use 24

cd frontend
npm install
npm run dev
```

前端将在 `http://localhost:5173` 启动。

## 使用流程

### 1. 注册和登录
1. 访问 `http://localhost:5173`
2. 点击"注册新账号"
3. 填写用户名、密码并选择角色（管理员/普通用户）
4. 注册成功后自动登录

### 2. 管理员操作

#### 2.1 项目管理
1. 访问管理员页面：以管理员身份登录后，点击头像或导航到 `/admin/projects`
2. 创建项目：
   - 点击"创建项目"按钮
   - 填写项目名称和描述
   - 提交后项目即可被用户看到
3. 编辑/删除项目：在项目列表中操作

#### 2.2 队列管理
1. 进入队列管理：点击左侧菜单"队列管理"
2. 筛选队列：可以按项目筛选队列列表
3. 创建队列：
   - 点击"创建队列"按钮
   - 选择所属项目
   - 输入队列名称
   - 设置对比图片数（2-10张）- 这决定了每组同时显示多少张图片供用户选择
4. 编辑队列：修改队列名称和对比图片数
5. 导入图片：点击"导入图片"按钮进入图片导入页面
6. 删除队列：删除队列会同时删除相关的所有图片和选择记录

#### 2.3 批量导入图片
1. 进入导入页面：
   - 在队列管理页面点击"导入图片"按钮
   - 系统会自动创建与"对比图片数"相同数量的文件夹上传区域

2. 为每个文件夹命名：
   - 为每个文件夹输入一个有意义的名称（如：method_a, method_b, original 等）
   - 文件夹名称不能重复
   - 这些名称将在用户选择时显示

3. 上传图片到各个文件夹：
   - 在每个文件夹的上传区域，上传对应的图片
   - **重要**：不同文件夹中的**同名图片**会被自动分组用于对比
   - 例如：
     - method_a 文件夹：`image1.jpg`, `image2.jpg`, `image3.jpg`
     - method_b 文件夹：`image1.jpg`, `image2.jpg`, `image3.jpg`
     - 系统会生成 3 个对比组：image1组、image2组、image3组

4. 查看预览：
   - 系统会显示所有图片组的预览表格
   - 绿色勾表示该文件在此文件夹中存在
   - 红色叉表示该文件缺失
   - "完整"标签表示该组在所有文件夹中都有对应文件

5. 提交导入：
   - 确认所有文件夹都有图片且已命名
   - 点击"开始导入"上传到服务器
   - 如有不完整的图片组，系统会提示确认

#### 2.4 数据导出
1. 进入数据导出：点击左侧菜单"数据导出"
2. 导出选择记录：
   - 选择要导出的队列
   - 选择导出格式（CSV 或 JSON）
   - 点击"导出选择记录"下载文件
   - 包含：用户ID、用户名、图片组、选择的文件夹、文件名、时间
3. 导出进度数据：
   - 可选择特定队列或导出所有队列
   - 选择导出格式（CSV 或 JSON）
   - 点击"导出进度数据"下载文件
   - 包含：队列信息、用户信息、完成数、总数、进度百分比
4. 快速导出：在数据统计表格中直接点击"导出选择"或"导出进度"按钮（默认CSV格式）

### 3. 用户操作
1. 查看项目：登录后自动进入项目列表
2. 选择队列：点击项目查看其下的队列
3. 开始选择：
   - 点击"开始选择"进入图片对比界面
   - 系统会显示同一组的多张图片（来自不同文件夹）
   - 点击选择一张图片
   - 提交后自动加载下一组
4. 查看进度：顶部进度条显示已完成/总数

## API 端点

### 认证
- `POST /api/auth/register` - 用户注册
- `POST /api/auth/login` - 用户登录

### 项目管理
- `GET /api/projects` - 获取所有项目
- `POST /api/projects` - 创建项目（管理员）
- `PUT /api/projects/{id}` - 更新项目（管理员）
- `DELETE /api/projects/{id}` - 删除项目（管理员）

### 队列管理
- `GET /api/queues?projectId={id}` - 获取队列列表
- `POST /api/queues` - 创建队列（管理员）
- `PUT /api/queues/{id}` - 更新队列（管理员）
- `DELETE /api/queues/{id}` - 删除队列（管理员）

### 图片管理
- `GET /api/images/next-group/{queueId}` - 获取下一组待选择图片
- `POST /api/images/import` - 导入图片（管理员）

### 选择记录
- `POST /api/selections` - 提交选择
- `GET /api/selections/progress/{queueId}` - 获取用户进度
- `GET /api/selections/queue/{queueId}` - 获取队列的选择记录

### 数据导出
- `GET /api/export/selections?queueId={id}&format=csv|json` - 导出选择数据
- `GET /api/export/progress?queueId={id}&format=csv|json` - 导出进度数据

## 项目结构

```
final/
├── Backend/                    # 后端项目
│   ├── Controllers/           # API 控制器
│   ├── Models/                # 数据模型
│   ├── DTOs/                  # 数据传输对象
│   ├── Data/                  # 数据库上下文
│   ├── Services/              # 服务层
│   └── Program.cs             # 程序入口
│
├── frontend/                   # 前端项目
│   ├── src/
│   │   ├── api/              # API 封装
│   │   ├── stores/           # Pinia 状态管理
│   │   ├── router/           # 路由配置
│   │   ├── views/            # 页面组件
│   │   │   ├── admin/        # 管理员页面
│   │   │   └── user/         # 用户页面
│   │   ├── types/            # TypeScript 类型
│   │   └── App.vue           # 根组件
│   └── vite.config.ts        # Vite 配置
│
└── README.md                   # 项目说明
```

## 数据库表结构

- **Users**: 用户表（ID, 用户名, 密码哈希, 角色）
- **Projects**: 项目表（ID, 名称, 描述, 创建者）
- **Queues**: 队列表（ID, 项目ID, 名称, 对比图片数）
- **Images**: 图片表（ID, 队列ID, 图片组, 文件夹名, 文件名, 路径）
- **SelectionRecords**: 选择记录表（ID, 队列ID, 用户ID, 图片组, 选中图片ID）
- **UserProgress**: 进度表（ID, 队列ID, 用户ID, 已完成数, 总数）

## 注意事项

1. **图片导入格式**: 
   - 从多个文件夹导入同名图片
   - 同名图片会被分组在一起进行对比
   - 示例：folder1/img1.jpg, folder2/img1.jpg 会作为一组

2. **对比数量配置**: 
   - 每个队列可以独立配置同时对比的图片数量（2-10张）
   - 例如设置为3，则每次显示3张来自不同文件夹的同名图片

3. **进度追踪**: 
   - 系统自动记录每个用户在每个队列中的进度
   - 用户可以随时中断，下次继续从未完成的图片组开始

4. **权限控制**: 
   - 管理员可以管理项目、队列，导入图片，导出数据
   - 普通用户只能查看项目和进行图片选择

## 开发说明

### 后端开发
```bash
cd Backend
dotnet watch run  # 热重载模式
```

### 前端开发
```bash
cd frontend
npm run dev  # 开发服务器
npm run build  # 生产构建
```

## 故障排除

### 数据库连接失败
- 检查 MySQL 服务是否运行
- 确认 `appsettings.json` 中的连接字符串正确
- 确保防火墙允许数据库连接

### 前端无法连接后端
- 确认后端正在运行在 `localhost:5000`
- 检查 CORS 配置是否正确
- 查看浏览器控制台的网络请求

### Node.js 版本问题
- 确保使用 Node.js 24: `nvs use 24`
- 如果 nvs 未安装，使用 nvm 或直接安装 Node.js 24

## 许可证

MIT License

