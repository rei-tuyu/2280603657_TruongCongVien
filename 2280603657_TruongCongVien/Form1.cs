using _2280603657_TruongCongVien.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2280603657_TruongCongVien
{
    public partial class frmQLSV : Form
    {
        public frmQLSV()
        {
            InitializeComponent();
        }
        // Event FormClosing có hiện thông báo xác nhận
        private void frmQLSV_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Thoát không?", "Xác nhận thoát",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        // Khi ấn button Thoát thì sẽ gọi Event FormClosing thực hiện
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // Lấy dữ liệu từ bảng Faculty của Database
        private void FillcbbFaculty(List<FACULTY> listFaculties)
        {
            cbbKhoa.DataSource = listFaculties;
            cbbKhoa.DisplayMember = "FacultyName";
            cbbKhoa.ValueMember = "FacultyID";
        }
        // Lấy dữ liệu từ bảng Students của database
        private void BindGrid(List<STUDENT> listStudents)
        {
            dgvStudent.Rows.Clear();
            foreach (STUDENT student in listStudents)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = student.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = student.StudentName;
                dgvStudent.Rows[index].Cells[2].Value = student.FACULTY.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = student.AverageScore;
            }
        }
        // Event FormLoad thực hiện các chức năng lấy thông tin
        private void frmQLSV_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<FACULTY> listFaculties = context.FACULTies.ToList();
                List<STUDENT> listStudents = context.STUDENTS.ToList();
                FillcbbFaculty(listFaculties);
                cbbKhoa.SelectedIndex = 0;
                BindGrid(listStudents);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Thêm 1 sinh viên vào database
        private void AddStudentToDatabase(STUDENT newStudent)
        {
            using (Model1 context = new Model1())
            {
                context.STUDENTS.Add(newStudent);
                context.SaveChanges();
            }
        }
        // Lấy sinh viên cuối cùng của database load lên DataGridView
        private void LoadData()
        {
            using (Model1 context = new Model1())
            {

                List<STUDENT> listStudents = context.STUDENTS.ToList();
                BindGrid(listStudents);
            }
        }
        // Reset Form để tiếp tục nhập
        private void ResetForm()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtDTB.Clear();
            cbbKhoa.SelectedIndex = 0;
        }
        // Khi ấn vào button Thêm sẽ kiểm tra các thông tin có được nhập đúng hay chưa và đưa ra thông báo
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMSSV.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                }
                else if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên và điểm trung bình!");

                }
                else if (string.IsNullOrWhiteSpace(txtDTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập điểm trung bình!");
                }
                else if (!float.TryParse(txtDTB.Text.Trim(), out float avgScore) || avgScore < 0 || avgScore > 10)
                {
                    MessageBox.Show("Điểm trung bình từ 0.00 đến 10.00!");
                    txtDTB.Clear();
                }
                STUDENT newStudent = new STUDENT
                {
                    StudentID = txtMSSV.Text.Trim(),
                    StudentName = txtHoTen.Text.Trim(),
                    AverageScore = float.Parse(txtDTB.Text),
                    FacultyID = (int)cbbKhoa.SelectedValue,
                };
                AddStudentToDatabase(newStudent);
                LoadData();
                ResetForm();
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Sửa sinh viên đã có trong danh sách bằng MSSV
        private void btnSua_Click(object sender, EventArgs e)
        {
            using (Model1 context = new Model1())
            {
                // Lấy mã số sinh viên từ TextBox
                string studentID = txtMSSV.Text.Trim();

                // Tìm sinh viên trong cơ sở dữ liệu
                var student = context.STUDENTS.SingleOrDefault(s => s.StudentID == studentID);

                if (student != null)
                {
                    // Cập nhật thông tin sinh viên
                    student.StudentName = txtHoTen.Text.Trim();
                    student.AverageScore = float.Parse(txtDTB.Text.Trim());
                    student.FacultyID = (int)cbbKhoa.SelectedValue;

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    context.SaveChanges();

                    // Tải lại dữ liệu lên DataGridView
                    LoadData();
                    ResetForm();
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên với mã số này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvStudent.SelectedRows.Count > 0)
            {
                // Lấy mã số sinh viên từ hàng được chọn trong DataGridView
                string studentID = dgvStudent.SelectedRows[0].Cells[0].Value.ToString();

                using (Model1 context = new Model1())
                {
                    // Tìm sinh viên trong cơ sở dữ liệu
                    var student = context.STUDENTS.SingleOrDefault(s => s.StudentID == studentID);

                    if (student != null)
                    {
                        // Xóa sinh viên khỏi cơ sở dữ liệu
                        context.STUDENTS.Remove(student);
                        context.SaveChanges();

                        // Xóa sinh viên khỏi DataGridView
                        dgvStudent.Rows.RemoveAt(dgvStudent.SelectedRows[0].Index);

                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên với mã số này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
