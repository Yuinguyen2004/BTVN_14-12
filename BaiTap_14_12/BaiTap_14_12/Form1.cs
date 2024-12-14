using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BaiTap_14_12.Model;

namespace BaiTap_14_12
{
    public partial class Form1 : Form
    {
        private StudentDBcontext db;
        public Form1()
        {
            InitializeComponent();
            db = new StudentDBcontext();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadFacultyData();
        }

        private void LoadData()
        {            
            var students = db.STUDENTs.Select(s => new
            {
                s.StudentID,
                s.Fullname,
                s.AverageScore,
                s.FacultyID,
                Khoa = s.FACULTY.FacultyName,
            }).ToList();

            dataGridView1.DataSource = students;
        }

        private void LoadFacultyData()
        {
            var khoa = db.FACULTies.Select(f => new
            {
                f.FacultyID,
                f.FacultyName
            }).ToList();
            cb_khoa.DataSource = khoa;
            cb_khoa.DisplayMember = "FacultyName";
            cb_khoa.ValueMember = "FacultyID";
        }

        private void bt_add_Click(object sender, EventArgs e)
        {
            var data = new STUDENT
            {
                StudentID = int.Parse(tb_mssv.Text),
                Fullname = tb_fullname.Text,
                AverageScore = float.Parse(tb_diemtb.Text),
                FacultyID = int.Parse(cb_khoa.SelectedValue.ToString()),
            };


            // Thêm vào cơ sở dữ liệu
            db.STUDENTs.Add(data);
            db.SaveChanges();

             // Cập nhật lại danh sách hiển thị
             LoadData();
             MessageBox.Show("Thêm sinh viên thành công!");
            }

        private void bt_xoa_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                int studentID = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                var student = db.STUDENTs.Find(studentID);
                if(student != null)
                {
                    //xóa khỏi cơ sở dữ liệu
                    db.STUDENTs.Remove(student);
                    db.SaveChanges();
                    //load lại cơ sở dữ liệu
                    LoadData();
                    MessageBox.Show("Xóa sinh viên thành công!");
                }    
            }    
        }

        private void bt_edit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int studentId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                // Tìm sinh viên cần sửa
                var student = db.STUDENTs.Find(studentId);
                if (student != null)
                {
                    student.Fullname = tb_fullname.Text;
                    student.AverageScore = float.Parse(tb_diemtb.Text);
                    student.FacultyID = int.Parse(cb_khoa.SelectedValue.ToString());

                    db.SaveChanges();

                    // Cập nhật lại danh sách hiển thị
                    LoadData();
                    MessageBox.Show("Cập nhật sinh viên thành công!");
                }
            }
        }
    }
    }

