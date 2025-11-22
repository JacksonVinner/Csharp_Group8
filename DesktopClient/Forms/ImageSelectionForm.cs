using ImageAnnotationApp.Services;
using ImageAnnotationApp.Models;

namespace ImageAnnotationApp.Forms
{
    public partial class ImageSelectionForm : Form
    {
        private readonly ImageService _imageService;
        private readonly SelectionService _selectionService;
        private readonly int _queueId;
        private readonly string _queueName;
        private ImageGroup? _currentGroup;
        private int _selectedImageId = -1;
        private Panel panelImages = null!;
        private ProgressBar progressBar = null!;
        private Label lblProgress = null!;
        private Button btnSubmit = null!;
        private ToolStripButton btnBack = null!;
        private CheckBox chkAutoSubmit = null!;

        public ImageSelectionForm(int queueId, string queueName)
        {
            _imageService = new ImageService();
            _selectionService = new SelectionService();
            _queueId = queueId;
            _queueName = queueName;
            InitializeCustomComponents();
            // 异步加载，不等待，避免阻塞UI
            _ = LoadNextGroupAsync();
        }

        private void InitializeCustomComponents()
        {
            this.Text = $"图片选择 - {_queueName}";
            this.Size = new Size(1000, 700);

            // 顶部工具栏
            var toolStrip = new ToolStrip();
            btnBack = new ToolStripButton("返回");
            btnBack.Click += BtnBack_Click;
            toolStrip.Items.Add(btnBack);

            // 进度条
            progressBar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 25,
                Style = ProgressBarStyle.Continuous
            };

            lblProgress = new Label
            {
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30,
                Text = "加载中..."
            };

            // 图片显示面板
            panelImages = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // 底部控制面板
            var panelBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            chkAutoSubmit = new CheckBox
            {
                Text = "自动提交",
                Location = new Point(10, 15),
                AutoSize = true
            };

            btnSubmit = new Button
            {
                Text = "提交选择",
                Location = new Point(150, 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSubmit.Click += async (s, e) => await SubmitSelectionAsync();

            panelBottom.Controls.Add(chkAutoSubmit);
            panelBottom.Controls.Add(btnSubmit);

            this.Controls.Add(panelImages);
            this.Controls.Add(panelBottom);
            this.Controls.Add(lblProgress);
            this.Controls.Add(progressBar);
            this.Controls.Add(toolStrip);
        }

        private async Task LoadNextGroupAsync()
        {
            try
            {
                panelImages.Controls.Clear();
                lblProgress.Text = "加载中...";

                // 获取进度
                var progress = await _selectionService.GetProgressAsync(_queueId);
                if (progress != null)
                {
                    progressBar.Maximum = progress.TotalGroups;
                    progressBar.Value = progress.CompletedGroups;
                    lblProgress.Text = $"进度: {progress.CompletedGroups}/{progress.TotalGroups} ({progress.ProgressPercentage:F1}%)";
                }

                // 获取下一组图片
                _currentGroup = await _imageService.GetNextGroupAsync(_queueId);

                if (_currentGroup == null || _currentGroup.Images.Count == 0)
                {
                    MessageBox.Show("恭喜！您已完成所有图片标注！", "完成",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BtnBack_Click(null, EventArgs.Empty);
                    return;
                }

                // 显示图片
                DisplayImages();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayImages()
        {
            if (_currentGroup == null) return;

            panelImages.Controls.Clear();
            _selectedImageId = -1;

            int imageCount = _currentGroup.Images.Count;
            int cols = (int)Math.Ceiling(Math.Sqrt(imageCount));
            int rows = (int)Math.Ceiling((double)imageCount / cols);

            int panelWidth = panelImages.ClientSize.Width - 20;
            int panelHeight = panelImages.ClientSize.Height - 20;
            int imageWidth = panelWidth / cols - 10;
            int imageHeight = panelHeight / rows - 30; // 减少高度，为标签留出空间
            int labelHeight = 20; // 标签高度

            for (int i = 0; i < _currentGroup.Images.Count; i++)
            {
                var image = _currentGroup.Images[i];
                int row = i / cols;
                int col = i % cols;

                int x = col * (imageWidth + 10) + 10;
                int y = row * (imageHeight + labelHeight + 10) + 10;

                // 创建图片容器面板
                var imagePanel = new Panel
                {
                    Size = new Size(imageWidth, imageHeight + labelHeight),
                    Location = new Point(x, y),
                    BorderStyle = BorderStyle.None
                };

                var pictureBox = new PictureBox
                {
                    Size = new Size(imageWidth, imageHeight),
                    Location = new Point(0, 0),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = image,
                    Cursor = Cursors.Hand
                };

                pictureBox.Click += PictureBox_Click;

                // 创建标签显示文件夹名
                var lblFolderName = new Label
                {
                    Text = image.FolderName,
                    Size = new Size(imageWidth, labelHeight),
                    Location = new Point(0, imageHeight),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Microsoft YaHei UI", 9F),
                    ForeColor = Color.DarkGray
                };

                imagePanel.Controls.Add(pictureBox);
                imagePanel.Controls.Add(lblFolderName);

                // 异步加载图片
                LoadImageAsync(pictureBox, image.FilePath);

                panelImages.Controls.Add(imagePanel);
            }
        }

        private async void LoadImageAsync(PictureBox pictureBox, string imagePath)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"开始加载图片: {imagePath}");
                
                var imageData = await _imageService.GetImageDataAsync(imagePath);
                if (imageData == null || imageData.Length == 0)
                {
                    pictureBox.BackColor = Color.LightGray;
                    pictureBox.Text = "图片数据为空";
                    System.Diagnostics.Debug.WriteLine($"图片数据为空: {imagePath}");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"图片数据大小: {imageData.Length} 字节");

                // 使用 MemoryStream 加载图片
                // 注意：对于 MemoryStream，数据已经在内存中，不需要保持流打开
                using (var ms = new MemoryStream(imageData))
                {
                    // 释放旧的图片资源
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                    }
                    
                    // 从流中加载图片
                    pictureBox.Image = System.Drawing.Image.FromStream(ms, false, false);
                }
                
                System.Diagnostics.Debug.WriteLine($"图片加载成功: {imagePath}");
            }
            catch (Exception ex)
            {
                // 加载失败时显示占位符和错误信息
                pictureBox.BackColor = Color.LightGray;
                var errorMsg = ex.Message.Length > 50 ? ex.Message.Substring(0, 50) + "..." : ex.Message;
                pictureBox.Text = $"加载失败\n{errorMsg}";
                System.Diagnostics.Debug.WriteLine($"图片加载失败: {imagePath}, 错误: {ex.Message}\n堆栈: {ex.StackTrace}");
            }
        }

        private async void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender is not PictureBox pictureBox || pictureBox.Tag is not Models.Image image)
                return;

            // 清除之前的所有选择（确保只选择一张图片）
            foreach (Control control in panelImages.Controls)
            {
                if (control is Panel imagePanel)
                {
                    // 查找 Panel 中的 PictureBox
                    foreach (Control child in imagePanel.Controls)
                    {
                        if (child is PictureBox pb)
                        {
                            pb.BorderStyle = BorderStyle.FixedSingle;
                            pb.BackColor = Color.Transparent;
                        }
                    }
                    imagePanel.BackColor = Color.Transparent;
                }
            }

            // 标记当前选择
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.BackColor = Color.LightGreen;
            
            // 如果 PictureBox 的父控件是 Panel，也设置 Panel 的背景色
            if (pictureBox.Parent is Panel parentPanel)
            {
                parentPanel.BackColor = Color.LightBlue;
            }
            
            _selectedImageId = image.Id;

            // 自动提交
            if (chkAutoSubmit.Checked)
            {
                await Task.Delay(100); // 短暂延迟
                await SubmitSelectionAsync();
            }
        }

        private async Task SubmitSelectionAsync()
        {
            if (_selectedImageId == -1)
            {
                MessageBox.Show("请选择一张图片", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_currentGroup == null) return;

            try
            {
                btnSubmit.Enabled = false;
                btnSubmit.Text = "提交中...";

                var dto = new CreateSelectionDto
                {
                    QueueId = _queueId,
                    ImageGroup = _currentGroup.GroupName,
                    SelectedImageId = _selectedImageId
                };

                await _selectionService.CreateAsync(dto);

                // 加载下一组
                await LoadNextGroupAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提交失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSubmit.Enabled = true;
                btnSubmit.Text = "提交选择";
            }
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            if (this.FindForm() is MainForm mainForm)
            {
                var panel = mainForm.Controls.Find("panelMain", true).FirstOrDefault();
                panel?.Controls.Clear();
                var queueListForm = new QueueListForm(0, "所有项目"); // 简化版，实际应该传递正确的项目ID
                queueListForm.TopLevel = false;
                queueListForm.FormBorderStyle = FormBorderStyle.None;
                queueListForm.Dock = DockStyle.Fill;
                panel?.Controls.Add(queueListForm);
                queueListForm.Show();
            }
        }
    }
}
