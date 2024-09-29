using ProductManagement.Models;
using ProductManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProductManagement
{
    public partial class MainForm : Form
    {
        private readonly ProductService _productService;
        private readonly ProductDTO _currentProduct;

        public MainForm()
        {
            InitializeComponent();
            _productService = new ProductService();
            _currentProduct = new ProductDTO();
        }

        private void btn_AddProd_Click(object sender, System.EventArgs e)
        {
            ResetFormField();
        }

        private void btn_SaveProd_Click(object sender, System.EventArgs e)
        {
            if (!IsFormProductValid())
            {
                MessageBox.Show("Có trường thông tin còn trống, vui lòng nhập đủ");
                return;
            }
            _currentProduct.ProdID = GetStringValue(tb_ProdID.Text);
            _currentProduct.ProdName = GetStringValue(tb_ProdName.Text);
            _currentProduct.Quantity = int.Parse(GetStringValue(txt_Quantity.Text));
            _currentProduct.Price = decimal.Parse(GetStringValue(tb_Price.Text));
            _currentProduct.Origin = GetStringValue(tb_Origin.Text);
            _currentProduct.ExpireDate = dtp_ExpireDate.Value;

            _productService.SaveProduct(_currentProduct);
            MessageBox.Show("Lưu thông tin SP thành công");
            LoadProductData();
        }

        private void btn_DeleteProd_Click(object sender, System.EventArgs e)
        {
            if (_currentProduct.ProdID == string.Empty)
            {
                MessageBox.Show("Trường mã SP chưa có, không thể xóa");
                return;
            }

            if (MessageBox.Show("Có muốn xóa SP này không ?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            _productService.DeleteProduct(p => p.ProdID == _currentProduct.ProdID);
            LoadProductData();
            ResetFormField();
            MessageBox.Show("Xóa SP thành công!");
        }

        private void Btn_MaxPriceProd_Click(object sender, System.EventArgs e)
        {
            var maxPriceProd = _productService.GetMaxPriceProduct();
            var products = new List<ProductDTO>
            {
                maxPriceProd
            };
            SetGridDataSource(dt_GridSearch, products);
        }

        private void Btn_JPProd_Click(object sender, System.EventArgs e)
        {
            var jpProducts = _productService.GetAllProducts(p => p.Origin == "Nhật Bản");
            SetGridDataSource(dt_GridSearch, jpProducts);
        }

        private void Btn_ExpProd_Click(object sender, System.EventArgs e)
        {
            var expProducts = _productService.GetAllProducts(p => p.ExpireDate <= DateTime.Now);
            SetGridDataSource(dt_GridSearch, expProducts);
        }

        private void Btn_RangePriceProd_Click(object sender, System.EventArgs e)
        {
            if (!IsRangePriceValid())
            {
                MessageBox.Show("Khoảng giá thiếu giá trị đầu hoặc cuối chưa nhập");
                return;
            }
            var startPrice = decimal.Parse(tb_StartPrice.Text);
            var endPrice = decimal.Parse(tb_EndPrice.Text);
            var rangedProducts = _productService
                .GetAllProducts(
                p => p.Price >= startPrice
                && p.Price <= endPrice);
            SetGridDataSource(dt_GridSearch, rangedProducts);
        }

        private void Btn_DeleteOrg_Click(object sender, System.EventArgs e)
        {
            string org = txt_DeleteOrg.Text;
            if (string.IsNullOrEmpty(org))
            {
                MessageBox.Show("Chưa nhập thông tin nguồn gốc xóa");
                return;
            }
            _productService.DeleteProduct(p => p.Origin == org);
            LoadProductData();
        }

        private void BtnCheckExp_Click(object sender, System.EventArgs e)
        {
            //var  = _productService.GetAllProducts(p => p.ExpireDate <= DateTime.Now);
            var numExpProducts = _productService.GetAllProducts(p => p.ExpireDate <= DateTime.Now).Count;

            if (numExpProducts > 0)
            {
                MessageBox.Show("Có " + numExpProducts + " sản phẩm hết hạn");
            }
            else MessageBox.Show("Không có sản phẩm hết hạn");
        }

        private void Btn_DeleteAllProd_Click(object sender, System.EventArgs e)
        {
            _productService.DeleteAllProduct();
            MessageBox.Show("Đã xóa hết sản phẩm");
            LoadProductData();
        }

        private void Btn_DeleteAllExp_Click(object sender, System.EventArgs e)
        {
            _productService.DeleteProduct(p => p.ExpireDate <= DateTime.Now);
            MessageBox.Show("Đã xóa hết sản phẩm hết hạn");
            LoadProductData();
        }

        private void Dt_GridProd_SelectionChanged(object sender, System.EventArgs e)
        {
            if (dt_GridProd.SelectedRows.Count > 0)
            {
                var selectedRow = dt_GridProd.SelectedRows[0];
            }
            else
            {
                return;
            }
            _currentProduct.ProdID = (string)dt_GridProd.CurrentRow.Cells[0].Value;

            var selectedProd = _productService.GetAllProducts().FirstOrDefault(
                p => p.ProdID == _currentProduct.ProdID);

            if (selectedProd == null || string.IsNullOrEmpty(selectedProd.ProdID))
            {
                return;
            }
            _currentProduct.ProdName = selectedProd.ProdName.Trim();
            _currentProduct.Quantity = selectedProd.Quantity;
            _currentProduct.Price = selectedProd.Price;
            _currentProduct.Origin = selectedProd.Origin.Trim();
            _currentProduct.ExpireDate = selectedProd.ExpireDate;
            SetFormData(_currentProduct);

            btn_DeleteProd.Enabled = true;
        }

        private void SetFormData(ProductDTO product)
        {
            tb_ProdID.Text = product.ProdID;
            tb_ProdName.Text = product.ProdName;
            tb_Price.Text = product.Price.ToString();
            txt_Quantity.Text = product.Quantity.ToString();
            tb_Origin.Text = product.Origin.ToString();
            dtp_ExpireDate.Value = product.ExpireDate;
        }

        private void Tb_Quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.Handled = false;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Tb_Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Tb_StartPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Tb_EndPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }


        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadProductData();
        }

        private bool IsFormProductValid()
        {
            if (GetStringValue(tb_ProdID.Text) == string.Empty)
            {
                MessageBox.Show("Vui lòng điền mã SP.");
                return false;
            }
            if (GetStringValue(tb_ProdName.Text) == string.Empty)
            {
                MessageBox.Show("Vui lòng điền tên SP.");
                return false;
            }
            if (GetStringValue(txt_Quantity.Text) == string.Empty)
            {
                MessageBox.Show("Vui lòng điền số lượng.");
                return false;
            }
            if (GetStringValue(tb_Price.Text) == string.Empty)
            {
                MessageBox.Show("Vui lòng điền đơn giá.");
                return false;
            }
            if (GetStringValue(tb_Origin.Text) == string.Empty)
            {
                MessageBox.Show("Vui lòng điền xuất xứ.");
                return false;
            }
            if (dtp_ExpireDate.Value == null)
            {
                MessageBox.Show("Chưa chọn ngày hết hạn.");
                return false;
            }
            return true;
        }

        private bool IsRangePriceValid()
        {
            if (GetStringValue(tb_StartPrice.Text) == string.Empty)
            {
                MessageBox.Show("Vui lòng điền giá trị đầu khoảng giá.");
                return false;
            }
            if (GetStringValue(tb_EndPrice.Text) == string.Empty)
            {
                MessageBox.Show("Vui lòng điền giá trị cuối khoảng giá.");
                return false;
            }
            return true;
        }

        private void ResetFormField()
        {
            tb_ProdID.Text = string.Empty;
            tb_ProdName.Text = string.Empty;
            txt_Quantity.Text = string.Empty;
            tb_Price.Text = string.Empty;
            tb_Origin.Text = string.Empty;

            dtp_ExpireDate.Value = DateTime.Now;
            dt_GridProd.ClearSelection();
            dt_GridProd.CurrentCell = null;
            btn_DeleteProd.Enabled = false;
        }

        private void LoadProductData()
        {
            var productGridProds = _productService.GetAllProducts();
            var productGridSearches = _productService.GetAllProducts();
            SetGridDataSource(dt_GridProd, productGridProds);
            SetGridDataSource(dt_GridSearch, productGridSearches);
        }

        private void SetGridDataSource(DataGridView dt, List<ProductDTO> items)
        {
            dt.DataSource = null;
            dt.DataSource = items;
        }

        private string GetStringValue(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return input.Trim();
        }
    }
}
