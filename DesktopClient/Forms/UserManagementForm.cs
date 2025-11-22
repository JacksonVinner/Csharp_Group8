using ImageAnnotationApp.Services;
using ImageAnnotationApp.Models;

namespace ImageAnnotationApp.Forms
{
    public partial class UserManagementForm : Form
    {
        private readonly UserService _userService;
        private TabControl tabControl = null!;
        private ListView listViewGuests = null!;
        private ListView listViewAllUsers = null!;

        public UserManagementForm()
        {
            _userService = new UserService();
            InitializeCustomComponents();
            LoadUsersAsync();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "用户管理";
            this.Size = new Size(1000, 700);

            tabControl = new TabControl { Dock = DockStyle.Fill };

            // 待审核游客标签页
            var tabGuests = new TabPage("待审核游客");
            listViewGuests = CreateListView();
            var toolStripGuests = new ToolStrip();
            var btnApprove = new ToolStripButton("批准");
            var btnReject = new ToolStripButton("拒绝");
            var btnRefreshGuests = new ToolStripButton("刷新");

            btnApprove.Click += async (s, e) => await ApproveUserAsync();
            btnReject.Click += async (s, e) => await RejectUserAsync();
            btnRefreshGuests.Click += async (s, e) => await LoadGuestUsersAsync();

            toolStripGuests.Items.Add(btnApprove);
            toolStripGuests.Items.Add(btnReject);
            toolStripGuests.Items.Add(new ToolStripSeparator());
            toolStripGuests.Items.Add(btnRefreshGuests);

            tabGuests.Controls.Add(listViewGuests);
            tabGuests.Controls.Add(toolStripGuests);
            toolStripGuests.Dock = DockStyle.Top;

            // 所有用户标签页
            var tabAllUsers = new TabPage("所有用户");
            listViewAllUsers = CreateListView();
            var toolStripAll = new ToolStrip();
            var btnRefreshAll = new ToolStripButton("刷新");
            btnRefreshAll.Click += async (s, e) => await LoadAllUsersAsync();
            toolStripAll.Items.Add(btnRefreshAll);

            tabAllUsers.Controls.Add(listViewAllUsers);
            tabAllUsers.Controls.Add(toolStripAll);
            toolStripAll.Dock = DockStyle.Top;

            tabControl.TabPages.Add(tabGuests);
            tabControl.TabPages.Add(tabAllUsers);

            this.Controls.Add(tabControl);
        }

        private ListView CreateListView()
        {
            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            listView.Columns.Add("ID", 50);
            listView.Columns.Add("用户名", 200);
            listView.Columns.Add("角色", 150);
            listView.Columns.Add("注册时间", 200);
            return listView;
        }

        private async Task LoadUsersAsync()
        {
            await LoadGuestUsersAsync();
            await LoadAllUsersAsync();
        }

        private async Task LoadGuestUsersAsync()
        {
            try
            {
                listViewGuests.Items.Clear();
                var guests = await _userService.GetGuestUsersAsync();

                foreach (var user in guests)
                {
                    var item = new ListViewItem(user.Id.ToString());
                    item.SubItems.Add(user.Username);
                    item.SubItems.Add(GetRoleDisplayName(user.Role));
                    item.SubItems.Add(user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.Tag = user;
                    listViewGuests.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载待审核游客失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAllUsersAsync()
        {
            try
            {
                listViewAllUsers.Items.Clear();
                var users = await _userService.GetAllUsersAsync();

                foreach (var user in users)
                {
                    var item = new ListViewItem(user.Id.ToString());
                    item.SubItems.Add(user.Username);
                    item.SubItems.Add(GetRoleDisplayName(user.Role));
                    item.SubItems.Add(user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.Tag = user;
                    listViewAllUsers.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载用户列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ApproveUserAsync()
        {
            if (listViewGuests.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要批准的用户", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = listViewGuests.SelectedItems[0].Tag as UserDto;
            if (user == null) return;

            try
            {
                await _userService.ApproveUserAsync(user.Id);
                MessageBox.Show($"用户 {user.Username} 已批准", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"批准用户失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RejectUserAsync()
        {
            if (listViewGuests.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要拒绝的用户", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = listViewGuests.SelectedItems[0].Tag as UserDto;
            if (user == null) return;

            var result = MessageBox.Show(
                $"确定要拒绝用户 '{user.Username}' 吗？",
                "确认拒绝",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                await _userService.DeleteUserAsync(user.Id);
                MessageBox.Show($"用户 {user.Username} 已拒绝", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"拒绝用户失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetRoleDisplayName(string role)
        {
            return role switch
            {
                "Admin" => "管理员",
                "User" => "普通用户",
                "Guest" => "游客（待审核）",
                _ => role
            };
        }
    }
}
