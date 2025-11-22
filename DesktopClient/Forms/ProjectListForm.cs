using ImageAnnotationApp.Services;
using ImageAnnotationApp.Models;

namespace ImageAnnotationApp.Forms
{
    public partial class ProjectListForm : Form
    {
        private readonly ProjectService _projectService;
        private readonly AuthService _authService;
        private ListView listView = null!;
        private ToolStripButton btnRefresh = null!;

        public ProjectListForm()
        {
            _projectService = new ProjectService();
            _authService = AuthService.Instance;
            InitializeCustomComponents();
            LoadProjectsAsync();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "项目列表";
            this.Size = new Size(900, 600);

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
            listView.DoubleClick += ListView_DoubleClick;

            // 工具栏
            var toolStrip = new ToolStrip();
            btnRefresh = new ToolStripButton("刷新");
            btnRefresh.Click += async (s, e) => await LoadProjectsAsync();
            toolStrip.Items.Add(btnRefresh);

            this.Controls.Add(listView);
            this.Controls.Add(toolStrip);
            toolStrip.Dock = DockStyle.Top;
        }

        private async Task LoadProjectsAsync()
        {
            try
            {
                btnRefresh.Enabled = false;
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
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private void ListView_DoubleClick(object? sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                var project = listView.SelectedItems[0].Tag as Project;
                if (project != null)
                {
                    if (_authService.CurrentUser?.Role == "Guest")
                    {
                        MessageBox.Show("游客用户无法进入项目，请等待管理员审核。", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var queueListForm = new QueueListForm(project.Id, project.Name);
                    var parentForm = this.ParentForm;
                    if (parentForm is MainForm mainForm)
                    {
                        mainForm.Controls.Find("panelMain", true).FirstOrDefault()?.Controls.Clear();
                        queueListForm.TopLevel = false;
                        queueListForm.FormBorderStyle = FormBorderStyle.None;
                        queueListForm.Dock = DockStyle.Fill;
                        mainForm.Controls.Find("panelMain", true).FirstOrDefault()?.Controls.Add(queueListForm);
                        queueListForm.Show();
                    }
                }
            }
        }
    }
}
