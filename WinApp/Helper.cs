using GM_DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp
{
    public static class Helper
    {


        public static int FindIndexByValue(ComboBox cb, object targetValue)
        {
            // 1. Lấy DataSource và ép kiểu về List<ComboBoxItem>
            if (cb.DataSource is List<ComboboxModel> itemList)
            {
                // 2. Sử dụng LINQ để tìm đối tượng đầu tiên có Value khớp
                ComboboxModel targetItem = itemList.Where(x => x.value == targetValue.ToString()).FirstOrDefault();

                if (targetItem != null)
                {
                    return itemList.IndexOf(targetItem);
                }
            }
            return -1;
        }


        public static void SetDefaultSelection(ComboBox myComboBox,string selectedValue)
        {

            int targetIndex = FindIndexByValue(myComboBox, selectedValue);
            if (targetIndex >= 0)
            {
                myComboBox.SelectedIndex = targetIndex;
            }
           
        }




        public static string GetImageFullPath(string imageName)
        {
            string imgPath = ConfigurationManager.AppSettings["ImageLibaryPath"];
           // string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(imgPath, imageName);
            return imagePath;
        }

        public static void SetupActionColumns(DataGridView dataGridView,bool isHaveEdit,bool isHaveDelete)
        {
            if (isHaveEdit)
            {
                // Đảm bảo không gọi lại nếu các cột đã tồn tại
                if (dataGridView.Columns.Contains("colEdit"))
                {
                    return;
                }
                // --- 1. Tải và Thêm Cột Icon "Edit" ---
                string editImagePath = GetImageFullPath("edit-icon.png");
                Image editIcon = null;
                if (File.Exists(editImagePath))
                {
                    try
                    {
                        // Tải ảnh. Sử dụng using để đảm bảo tài nguyên được giải phóng đúng cách
                        using (var stream = new FileStream(editImagePath, FileMode.Open, FileAccess.Read))
                        {
                            editIcon = Image.FromStream(stream);
                        }

                        DataGridViewImageColumn colEdit = new DataGridViewImageColumn
                        {
                            HeaderText = "Sửa",
                            Name = "colEdit",
                            Image = editIcon,
                            ImageLayout = DataGridViewImageCellLayout.Zoom,
                            Width = 50,
                            ToolTipText = "Chỉnh sửa"
                        };
                        dataGridView.Columns.Insert(0, colEdit);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi tải ảnh Edit: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (dataGridView.Columns.Contains("colEdit")) dataGridView.Columns["colEdit"].ReadOnly = true;


            }
            
            if (isHaveDelete)
            {
                // Đảm bảo không gọi lại nếu các cột đã tồn tại
                if (dataGridView.Columns.Contains("colDelete"))
                {
                    return;
                }
                // --- 2. Tải và Thêm Cột Icon "Delete" ---
                string deleteImagePath = GetImageFullPath("delete-red-icon.png");
                Image deleteIcon = null;
                if (File.Exists(deleteImagePath))
                {
                    try
                    {
                        using (var stream = new FileStream(deleteImagePath, FileMode.Open, FileAccess.Read))
                        {
                            deleteIcon = Image.FromStream(stream);
                        }

                        DataGridViewImageColumn colDelete = new DataGridViewImageColumn
                        {
                            HeaderText = "Xóa",
                            Name = "colDelete",
                            Image = deleteIcon,
                            ImageLayout = DataGridViewImageCellLayout.Zoom,
                            Width = 40,
                            ToolTipText = "Xóa"
                        };
                        // Chèn vào vị trí 1 (sau cột Edit) hoặc 0 (nếu không có cột Edit)
                        int insertIndex = dataGridView.Columns.Contains("colEdit") ? 1 : 0;
                        dataGridView.Columns.Insert(insertIndex, colDelete);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi tải ảnh Delete: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (dataGridView.Columns.Contains("colDelete")) dataGridView.Columns["colDelete"].ReadOnly = true;
            }
           
           

           

            

            // Đảm bảo các cột action không được chỉnh sửa
           
           
        }


        public static void SetupFormatGridView(DataGridView dataGridView)
        {
            // dataGridView.EnableHeadersVisualStyles = true;
            dataGridView.AutoGenerateColumns = false;
            
            dataGridView.RowTemplate.Height = 40;
      
            dataGridView.Font = new Font(dataGridView.Font.FontFamily, 10f);
            dataGridView.ColumnHeadersDefaultCellStyle.Font =new Font(dataGridView.Font, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment =DataGridViewContentAlignment.MiddleCenter;
           
          //  dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.SkyBlue;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.SkyBlue;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

        }


        public static string ReFormatDate(string dateValue,string charPlit)
        {
            string newFormat = "";
            if (!string.IsNullOrEmpty(dateValue))
            {
                var arrMem = dateValue.Split(charPlit);
                if (arrMem.Length == 3)
                {
                    string mem1 = arrMem[0];
                    string mem2 = arrMem[1];
                    string mem3 = arrMem[2];
                    newFormat = mem3 + charPlit+mem2+charPlit+mem1;
                }
            }
            return newFormat;
        }


    }
}
