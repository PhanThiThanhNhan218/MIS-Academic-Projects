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
    public partial class frmQuanLyKho : Form
    {
        string sCon = "Data Source=DESKTOP-EG837TQ;Initial Catalog=QuanLyNhaThuoc;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        public frmQuanLyKho()
        {
            InitializeComponent();
            cboLGD.Items.Add("Nhập");
            cboLGD.Items.Add("Xuất");
            cboLGD.SelectedIndex = 0;
            nudSL.Minimum = 1;
        }

        private void frmQuanLyKho_Load(object sender, EventArgs e)
        {
            string sQuery = "select MaHH, SoLuongTon, TenHH from HANGHOA";

            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "HANGHOA");

                    dataGridView1.DataSource = ds.Tables["HANGHOA"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi trong quá trình kết nối DB: " + ex.Message);
                }
            }
        }

        private void btnKiemTra_Click(object sender, EventArgs e)
        {
            int soLuong = (int)nudSL.Value;
            string loaiGiaoDich = cboLGD.SelectedItem.ToString();
            string maHH = txtMaHH.Text;
            int soLuongToiThieu = 10;
            int soLuongToiDa = 100;

            using (SqlConnection con = new SqlConnection(sCon))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select MaHH,TenHH,SoLuongTon from HANGHOA where MaHH = @MaHH", con);
                cmd.Parameters.AddWithValue("@MaHH", maHH);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int soLuongTon = Convert.ToInt32(reader[2]);    
                    string tenHH = reader.GetString(1);

                    txtTenHH.Text = tenHH;

                    if (loaiGiaoDich == "Nhập")
                    {
                        if (soLuongTon + soLuong > soLuongToiDa)
                            MessageBox.Show("Từ chối nhập số lượng tồn kho vượt mức tối đa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Có thể nhập hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (loaiGiaoDich == "Xuất")
                    {
                        if (soLuongTon < soLuong)
                            MessageBox.Show("Không đủ hàng để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Có thể xuất hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Cảnh báo thêm
                    if (soLuongTon < soLuongToiThieu)
                        MessageBox.Show("Hàng sắp hết, cần nhập thêm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (soLuongTon > soLuongToiDa)
                        MessageBox.Show("Hàng tồn quá nhiều, cần hạn chế nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            string maHH = txtMaHH.Text.Trim();
            string loaiGD = cboLGD.SelectedItem.ToString();
            int soLuong = (int)nudSL.Value;

            int soLuongToiThieu = 10;
            int soLuongToiDa = 100;

            using (SqlConnection con = new SqlConnection(sCon))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select SoLuongTon from HangHoa where MaHH = @MaHH", con);
                cmd.Parameters.AddWithValue("@MaHH", maHH);
                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    MessageBox.Show("Không tìm thấy hàng hóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int soLuongTon = Convert.ToInt32(result);

                if (loaiGD == "Nhập")
                {
                    if (soLuongTon + soLuong > soLuongToiDa)
                    {
                        MessageBox.Show("Từ chối nhập thêm hàng, hàng vượt quá mức tối đa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        int soLuongMoi = soLuongTon + soLuong;
                        CapNhatSoLuong(con, maHH, soLuongMoi);
                        MessageBox.Show("Nhập hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show($"Số lượng tồn mới : {soLuongMoi}");

                    }
                }
                else if (loaiGD == "Xuất")
                {
                    if (soLuongTon < soLuong)
                    {
                        MessageBox.Show("Số lượng tồn không đủ để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        int soLuongMoi = soLuongTon - soLuong;
                        CapNhatSoLuong(con, maHH, soLuongMoi);
                        MessageBox.Show("Xuất hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show($"Số lượng tồn mới : {soLuongMoi}");
                    }
                }

                // Kiểm tra lại số lượng tồn mới
                SqlCommand cmdTonMoi = new SqlCommand("select SoLuongTon from HangHoa where MaHH = @MaHH", con);
                cmdTonMoi.Parameters.AddWithValue("@MaHH", maHH);
                int soLuongTonMoi = (int)cmdTonMoi.ExecuteScalar();

                if (soLuongTonMoi < soLuongToiThieu)
                    MessageBox.Show("Hàng sắp hết, cần nhập thêm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void CapNhatSoLuong(SqlConnection conn, string maHH, int soLuongMoi)
        {
            SqlCommand updateCmd = new SqlCommand("update HangHoa set SoLuongTon = @SoLuongTon where MaHH = @MaHH", conn);
            updateCmd.Parameters.AddWithValue("@SoLuongTon", soLuongMoi);
            updateCmd.Parameters.AddWithValue("@MaHH", maHH);
            updateCmd.ExecuteNonQuery();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
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
                nudSL.Text = row.Cells["Soluongton"].Value.ToString();

            }
        }
    }
}
