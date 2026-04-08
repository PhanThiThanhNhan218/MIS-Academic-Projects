using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace App1
{
    public partial class frmNhaCungCap : Form
    {
        string sCon = "Data Source=DESKTOP-EG837TQ;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình kết nối!");
            }
            string sQuery = "select * from NHACUNGCAP";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "NHACUNGCAP");
            dataGridView1.DataSource = ds.Tables["NHACUNGCAP"];
            con.Close();
        }
        private void LoadData()
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string sQuery = "SELECT * FROM NHACUNGCAP";
                SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string query = "INSERT INTO NHACUNGCAP (MaNCC, TenNCC, DiaChiNCC, SDTNCC) VALUES (@MaNCC, @TenNCC, @DiaChi, @SDT)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                cmd.Parameters.AddWithValue("@TenNCC", txtTenNCC.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChiNCC.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSDTNCC.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm nhà cung cấp thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm mới: " + ex.Message);
            }
            LoadData();

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string sQuery = "UPDATE NHACUNGCAP SET TenNCC=@TenNCC, DiaChiNCC=@DiaChi, SDTNCC=@SDT WHERE MaNCC=@MaNCC";
                SqlCommand cmd = new SqlCommand(sQuery, con);
                cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                cmd.Parameters.AddWithValue("@TenNCC", txtTenNCC.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChiNCC.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSDTNCC.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thông tin thành công!");
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
            con.Close();
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string query = "DELETE FROM NHACUNGCAP WHERE MaNCC=@MaNCC";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa nhà cung cấp thành công!");
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
            LoadData();
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtMaNCC.Text = dataGridView1.Rows[i].Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = dataGridView1.Rows[i].Cells["TenNCC"].Value.ToString();
                txtDiaChiNCC.Text = dataGridView1.Rows[i].Cells["DiaChiNCC"].Value.ToString();
                txtSDTNCC.Text = dataGridView1.Rows[i].Cells["SDTNCC"].Value.ToString();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNCC.Text = dataGridView1.Rows[e.RowIndex].Cells["MaNCC"].Value.ToString();
            txtTenNCC.Text = dataGridView1.Rows[e.RowIndex].Cells["TenNCC"].Value.ToString();
            txtSDTNCC.Text = dataGridView1.Rows[e.RowIndex].Cells["SDTNCC"].Value.ToString();
            txtDiaChiNCC.Text = dataGridView1.Rows[e.RowIndex].Cells["DiaChiNCC"].Value.ToString();

        }
    }
}
