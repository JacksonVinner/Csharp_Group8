using ImageAnnotationApp.Services;
using ImageAnnotationApp.Models;

namespace ImageAnnotationApp.Forms
{
    public partial class QueueListForm : Form
    {
        private readonly QueueService _queueService;
        private readonly int _projectId;
        private readonly string _projectName;
        private ListView listView = null!;
        private ToolStripButton btnRefresh = null!;
        private ToolStripButton btnBack = null!;

        public QueueListForm(int projectId, string projectName)
        {
            _queueService = new QueueService();
            _projectId = projectId;
            _projectName = projectName;
            InitializeCustomComponents();
            LoadQueuesAsync();
        }

        private void InitializeCustomComponents()
        {
            this.Text = $"队列列表 - {_projectName}";
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
            listView.Columns.Add("队列名称", 200);
            listView.Columns.Add("对比图片数", 100);
            listView.Columns.Add("总图片组", 100);
            listView.Columns.Add("创建时间", 150);
            listView.Columns.Add("", 150);

            // 工具栏
            var toolStrip = new ToolStrip();
            btnBack = new ToolStripButton("返回");
            btnBack.Click += BtnBack_Click;
            btnRefresh = new ToolStripButton("刷新");
            btnRefresh.Click += async (s, e) => await LoadQueuesAsync();
            toolStrip.Items.Add(btnBack);
            toolStrip.Items.Add(btnRefresh);

            this.Controls.Add(listView);
            this.Controls.Add(toolStrip);
            toolStrip.Dock = DockStyle.Top;

            listView.MouseDoubleClick += ListView_MouseDoubleClick;
            listView.DoubleClick += (s, e) => ListView_MouseDoubleClick(s, new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0));
        }

        private async Task LoadQueuesAsync()
        {
            try
            {
                btnRefresh.Enabled = false;
                listView.Items.Clear();

                var queues = await _queueService.GetAllAsync(_projectId);

                foreach (var queue in queues)
                {
                    var item = new ListViewItem(queue.Id.ToString());
                    item.SubItems.Add(queue.Name);
                    item.SubItems.Add(queue.ImageCount.ToString());
                    item.SubItems.Add(queue.TotalImages.ToString());
                    item.SubItems.Add(queue.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
                    item.SubItems.Add("双击开始选择");
                    item.Tag = queue;
                    listView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载队列列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private void ListView_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count == 0)
                {
                    return;
                }

                var queue = listView.SelectedItems[0].Tag as Models.Queue;
                if (queue == null)
                {
                    MessageBox.Show("无法获取队列信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 通过 Parent 向上查找 MainForm，或者从 Application.OpenForms 中查找
                MainForm? mainForm = null;
                
                // 方法1: 通过 Parent 向上查找
                Control? current = this.Parent;
                while (current != null)
                {
                    if (current is MainForm mf)
                    {
                        mainForm = mf;
                        break;
                    }
                    current = current.Parent;
                }

                // 方法2: 如果方法1失败，从 Application.OpenForms 中查找
                if (mainForm == null)
                {
                    mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                }

                if (mainForm == null)
                {
                    MessageBox.Show("无法找到主窗体", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var panel = mainForm.Controls.Find("panelMain", true).FirstOrDefault();
                if (panel == null)
                {
                    MessageBox.Show("无法找到主面板", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var imageSelectionForm = new ImageSelectionForm(queue.Id, queue.Name);
                panel.Controls.Clear();
                imageSelectionForm.TopLevel = false;
                imageSelectionForm.FormBorderStyle = FormBorderStyle.None;
                imageSelectionForm.Dock = DockStyle.Fill;
                panel.Controls.Add(imageSelectionForm);
                imageSelectionForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开图片选择界面失败: {ex.Message}\n\n堆栈跟踪:\n{ex.StackTrace}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            try
            {
                // 通过 Parent 向上查找 MainForm，或者从 Application.OpenForms 中查找
                MainForm? mainForm = null;
                
                // 方法1: 通过 Parent 向上查找
                Control? current = this.Parent;
                while (current != null)
                {
                    if (current is MainForm mf)
                    {
                        mainForm = mf;
                        break;
                    }
                    current = current.Parent;
                }

                // 方法2: 如果方法1失败，从 Application.OpenForms 中查找
                if (mainForm == null)
                {
                    mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                }

                if (mainForm == null)
                {
                    MessageBox.Show("无法找到主窗体", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var projectListForm = new ProjectListForm();
                var panel = mainForm.Controls.Find("panelMain", true).FirstOrDefault();
                if (panel != null)
                {
                    panel.Controls.Clear();
                    projectListForm.TopLevel = false;
                    projectListForm.FormBorderStyle = FormBorderStyle.None;
                    projectListForm.Dock = DockStyle.Fill;
                    panel.Controls.Add(projectListForm);
                    projectListForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"返回失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
