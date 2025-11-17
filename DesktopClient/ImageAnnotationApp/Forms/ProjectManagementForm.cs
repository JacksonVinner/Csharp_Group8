using ImageAnnotationApp.Services;
using ImageAnnotationApp.Models;

namespace ImageAnnotationApp.Forms
{
    public partial class ProjectManagementForm : Form
    {
        private readonly ProjectService _projectService;
        private ListView listView;

        public ProjectManagementForm()
        {
            _projectService = new ProjectService();
            InitializeCustomComponents();
            LoadProjectsAsync();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "项目管理";
            this.Size = new Size(1000, 700);

            // ListView
            listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            listView.Columns.Add("ID", 50);
            listView.Columns.Add("项目名称", 200);
            listView.Columns.Add("描述", 300);
            listView.Columns.Add("队列数", 80);
            listView.Columns.Add("创建者", 120);
            listView.Columns.Add("创建时间", 150);

            // 工具栏
            var toolStrip = new ToolStrip();
            var btnAdd = new ToolStripButton("新建项目");
            var btnEdit = new ToolStripButton("编辑");
            var btnDelete = new ToolStripButton("删除");
            var btnRefresh = new ToolStripButton("刷新");

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += async (s, e) => await BtnDelete_ClickAsync();
            btnRefresh.Click += async (s, e) => await LoadProjectsAsync();

            toolStrip.Items.Add(btnAdd);
            toolStrip.Items.Add(btnEdit);
            toolStrip.Items.Add(btnDelete);
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(btnRefresh);

            this.Controls.Add(listView);
            this.Controls.Add(toolStrip);
            toolStrip.Dock = DockStyle.Top;
        }

        private async Task LoadProjectsAsync()
        {
            try
            {
                listView.Items.Clear();
                var projects = await _projectService.GetAllAsync();

                foreach (var project in projects)
                {
                    var item = new ListViewItem(project.Id.ToString());
                    item.SubItems.Add(project.Name);
                    item.SubItems.Add(project.Description ?? "");
                    item.SubItems.Add(project.QueueCount.ToString());
                    item.SubItems.Add(project.CreatedByUsername);
                    item.SubItems.Add(project.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
                    item.Tag = project;
                    listView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载项目列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            var name = PromptInput("项目名称:", "新建项目");
            if (string.IsNullOrEmpty(name)) return;

            var description = PromptInput("项目描述:", "新建项目");

            try
            {
                await _projectService.CreateAsync(new CreateProjectDto
                {
                    Name = name,
                    Description = description
                });

                MessageBox.Show("项目创建成功", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadProjectsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建项目失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要编辑的项目", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var project = listView.SelectedItems[0].Tag as Project;
            if (project == null) return;

            var name = PromptInput("项目名称:", "编辑项目", project.Name);
            if (string.IsNullOrEmpty(name)) return;

            var description = PromptInput("项目描述:", "编辑项目", project.Description ?? "");

            try
            {
                await _projectService.UpdateAsync(project.Id, new UpdateProjectDto
                {
                    Name = name,
                    Description = description
                });

                MessageBox.Show("项目更新成功", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadProjectsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新项目失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task BtnDelete_ClickAsync()
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要删除的项目", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var project = listView.SelectedItems[0].Tag as Project;
            if (project == null) return;

            var result = MessageBox.Show(
                $"确定要删除项目 '{project.Name}' 吗？\n此操作将删除该项目及其所有队列和图片！",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                await _projectService.DeleteAsync(project.Id);
                MessageBox.Show("项目删除成功", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadProjectsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除项目失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string? PromptInput(string prompt, string title, string defaultValue = "")
        {
            var form = new Form
            {
                Text = title,
                Size = new Size(400, 150),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label { Text = prompt, Location = new Point(10, 20), AutoSize = true };
            var textBox = new TextBox { Location = new Point(10, 50), Size = new Size(360, 23), Text = defaultValue };
            var btnOk = new Button { Text = "确定", DialogResult = DialogResult.OK, Location = new Point(200, 80), Size = new Size(80, 30) };
            var btnCancel = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Location = new Point(290, 80), Size = new Size(80, 30) };

            form.Controls.AddRange(new Control[] { label, textBox, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            return form.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
    }
}
