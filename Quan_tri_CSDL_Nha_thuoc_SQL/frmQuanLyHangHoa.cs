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
    public partial class frmQunaLyHangHoa : Form
    {
        string sCon = "Data Source=DESKTOP-EG837TQ;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public frmQunaLyHangHoa()
        {
            InitializeComponent();
        }

        private void frmQunaLyHangHoa_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình kết nối DB");
            }
            string sQuery = "select * from HANGHOA";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "HANGHOA");
            dataGridView1.DataSource = ds.Tables["HANGHOA"];
            con.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maCantim = txtMaHH.Text;

            if (string.IsNullOrWhiteSpace(maCantim))
            {
                MessageBox.Show("Vui lòng nhập mã hàng hóa cần tìm.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(sCon))
                {
                    con.Open();
                    string sql = "select * from HangHoa where MaHH like @MaHH";
                    using (SqlCommand command = new SqlCommand(sql, con))
                    {
                        command.Parameters.AddWithValue("@MaHH", "%" + maCantim + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string sql = "insert into HangHoa (MaHH, TenHH, SoLuongTon, DVT, MaNhom) values (@MaHH, @TenHH, @SLT, @DVT, @MaNhom)";
                SqlCommand command = new SqlCommand(sql, con);

                command.Parameters.AddWithValue("@MaHH", txtMaHH.Text);
                command.Parameters.AddWithValue("@TenHH", txtTenHH.Text);
                command.Parameters.AddWithValue("@DVT", txtDVT.Text);
                int slt = int.Parse(txtSLT.Text);
                command.Parameters.AddWithValue("@SLT", slt);
                command.Parameters.AddWithValue("@MaNhom", txtMN.Text);

                command.ExecuteNonQuery();

                MessageBox.Show("Thêm hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình thêm mới: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
                string sql = "update HangHoa set MaHH=@MaHH, TenHH=@TenHH, SoLuongTon=@SLT, DVT=@DVT, MaNhom=@MaNhom where MaHH=@MaHH";
                SqlCommand command = new SqlCommand(sql, con);

                command.Parameters.AddWithValue("@MaHH", txtMaHH.Text);
                command.Parameters.AddWithValue("@TenHH", txtTenHH.Text);
                command.Parameters.AddWithValue("@DVT", txtDVT.Text);
                int slt = int.Parse(txtSLT.Text);
                command.Parameters.AddWithValue("@SLT", slt);
                command.Parameters.AddWithValue("@MaNhom", txtMN.Text);

                command.ExecuteNonQuery();

                MessageBox.Show("Đã sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xảy ra lỗi trong quá trình sửa: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maHH = txtMaHH.Text;

            if (string.IsNullOrWhiteSpace(maHH))
            {
                MessageBox.Show("Vui lòng nhập mã hàng hóa cần xóa.");
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa hàng hóa {maHH} không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(sCon))
                    {
                        con.Open();

                        string sql = "delete from HANGHOA where MaHH = @MaHH";
                        using (SqlCommand command = new SqlCommand(sql, con))
                        {
                            command.Parameters.AddWithValue("@MaHH", maHH);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Đã xóa hàng hóa thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy mã hàng hóa cần xóa.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy hàng được click
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Đổ dữ liệu từ các ô của hàng vào các textbox
                txtMaHH.Text = row.Cells["MaHH"].Value.ToString();
                txtTenHH.Text = row.Cells["TenHH"].Value.ToString();
                txtDVT.Text = row.Cells["DVT"].Value.ToString();
                txtSLT.Text = row.Cells["Soluongton"].Value.ToString();
                txtMN.Text = row.Cells["MaNhom"].Value.ToString();
            }
        }
        private void LoadDataToDataGridView()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sCon))
                {
                    con.Open();
                    string query = "select * from HANGHOA";
                    SqlCommand command = new SqlCommand(query, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    // Hiển thị tiêu đề cột
                    dataGridView1.ColumnHeadersVisible = true;

                    // Tùy chỉnh tiêu đề cột (nếu muốn)
                    dataGridView1.Columns["MaHH"].HeaderText = "Mã hàng hoá";
                    dataGridView1.Columns["TenHH"].HeaderText = "Tên hàng hoá";
                    dataGridView1.Columns["DVT"].HeaderText = "ĐVT";
                    dataGridView1.Columns["SLT"].HeaderText = "Số lượng tồn";
                    dataGridView1.Columns["MaNhom"].HeaderText = "Mã nhóm";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
    }
}
