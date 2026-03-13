using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq; // Để dùng hàm lọc dữ liệu

namespace _1303

{
    public partial class MainWindow:Window
    {
        List<SanPham> danhSachSP = new List<SanPham>();
        ObservableCollection<SanPham> danhSachSanPham = new ObservableCollection<SanPham>();

        public MainWindow()
        {
            InitializeComponent();

            // Gán dữ liệu cho bảng trên
            dgSanPham.ItemsSource = danhSachSanPham;
        }

        // 1. NÚT NHẬP: Dùng để làm mới (xóa trắng) các ô TextBox
        private void btnNhap_Click(object sender, RoutedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtMaSP.Text, @"^\d{4}$"))
            {
                MessageBox.Show("Mã sản phẩm phải bao gồm đúng 4 chữ số!");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtTenSP.Text, @"^[\p{L}\s]+$") ||
                !System.Text.RegularExpressions.Regex.IsMatch(txtNuocSX.Text, @"^[\p{L}\s]+$"))
            {
                MessageBox.Show("Tên sản phẩm và Nước sản xuất chỉ được phép chứa chữ!");
                return;
            }

            if (!int.TryParse(txtNamSX.Text, out int nam) || nam < 1980 || nam > 2026)
            {
                MessageBox.Show("Năm sản xuất phải là số và nằm trong khoảng từ 1980 đến 2026!");
                return;
            }

            danhSachSP.Add(new SanPham
            {
                No = danhSachSP.Count + 1,
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                NuocSX = txtNuocSX.Text,
                NamSX = nam
            });

            txtMaSP.Clear();
            txtTenSP.Clear();
            txtNuocSX.Clear();
            txtNamSX.Clear();

        }

        // 2. NÚT HIỂN THỊ: Lấy dữ liệu từ TextBox đẩy xuống bảng
        private void btnHienThi_Click(object sender, RoutedEventArgs e)
        {
            dgSanPham.ItemsSource = null;
            dgSanPham.ItemsSource = danhSachSP;
        }
        // 3. CHỌN SẢN PHẨM Ở BẢNG LÊN TEXTBOX
        private void dgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Kiểm tra xem có dòng nào đang được bấm chọn không
            if (dgSanPham.SelectedItem != null)
            {
                SanPham spChon = (SanPham)dgSanPham.SelectedItem;

                // Đẩy dữ liệu ngược lên TextBox
                txtMaSP.Text = spChon.MaSP;
                txtTenSP.Text = spChon.TenSP;
                txtNuocSX.Text = spChon.NuocSX;
                txtNamSX.Text = spChon.NamSX.ToString();
            }
        }

        // 4. NÚT LỌC: Các sản phẩm trước năm 2000
        private void btnLoc_Click(object sender, RoutedEventArgs e)
        {
            // Dùng LINQ để lọc các sản phẩm có Năm SX < 2000
            var danhSachLoc = danhSachSP.Where(sp => sp.NamSX < 2000).ToList();

            // Gán danh sách đã lọc vào bảng bên dưới
            dgSanPhamLoc.ItemsSource = danhSachLoc;
        }

        // 5. NÚT THOÁT
        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            // Tắt chương trình
            Application.Current.Shutdown();
        }

        // NÚT CHỌN SẢN PHẨM: Đẩy dữ liệu từ dòng đang chọn trên bảng lên TextBox
        private void btnChon_Click(object sender, RoutedEventArgs e)
        {
          
            // Đã sửa dgHienThiTatCa thành dgSanPham theo đúng tên bảng của bác
            if (dgSanPham.SelectedItems.Count > 0)
            {
                System.Text.StringBuilder thongBao = new System.Text.StringBuilder();
                thongBao.AppendLine($"BẠN ĐÃ CHỌN {dgSanPham.SelectedItems.Count} SẢN PHẨM:\n");

                // Duyệt qua các dòng được highlight chọn trong bảng
                foreach (var item in dgSanPham.SelectedItems)
                {
                    if (item is SanPham sp)
                    {
                        thongBao.AppendLine($"- Mã SP: {sp.MaSP} | Tên SP: {sp.TenSP} | Nước SX: {sp.NuocSX} | Năm SX: {sp.NamSX}");
                    }
                }

                MessageBox.Show(thongBao.ToString(), "Thông báo chọn sản phẩm");
            }
            else
            {
                MessageBox.Show("Vui lòng click chọn ít nhất một sản phẩm trong bảng!", "Thông báo");
            }
        }
    
    }


    // Class dữ liệu
    public class SanPham
    {
        public bool IsSelected { get; set; } // Phục vụ cho CheckBox ở cột No
        public int No { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string NuocSX { get; set; }
        public int NamSX { get; set; }
    }
}