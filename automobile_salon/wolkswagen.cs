using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using MaterialSkin.Animations;
using MaterialSkin.Controls;
using MaterialSkin;
using System.Drawing.Text;

namespace automobile_salon
{
    public partial class wolkswagen : MaterialForm
    {
        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        System.Windows.Forms.Timer timer;


        private string server;
        private string database;
        private string uid;
        private string password;
        public MySqlConnection connection;

        private BindingSource postSorce = new BindingSource();
        private BindingSource staffSorce = new BindingSource();
        private BindingSource customerSorce = new BindingSource();
        private BindingSource optionsSorce = new BindingSource();
        private BindingSource engineSorce = new BindingSource();
        private BindingSource bodySorce = new BindingSource();
        private BindingSource manufacturerSorce = new BindingSource();
        private BindingSource equipmentSorce = new BindingSource();
        private BindingSource brandSorce = new BindingSource();
        private BindingSource suuplySorce = new BindingSource();
        private BindingSource carstSorce = new BindingSource();
        private BindingSource ordersSorce = new BindingSource();
        private BindingSource contractSorce = new BindingSource();


        MySqlDataAdapter postAdapter = new MySqlDataAdapter();
        MySqlDataAdapter staffAdapter = new MySqlDataAdapter();
        MySqlDataAdapter customerAdapter = new MySqlDataAdapter();
        MySqlDataAdapter optionsAdapter = new MySqlDataAdapter();
        MySqlDataAdapter engineAdapter = new MySqlDataAdapter();
        MySqlDataAdapter bodyAdapter = new MySqlDataAdapter();
        MySqlDataAdapter manufacturerAdapter = new MySqlDataAdapter();
        MySqlDataAdapter equipmentAdapter = new MySqlDataAdapter();
        MySqlDataAdapter brandAdapter = new MySqlDataAdapter();
        MySqlDataAdapter suuplyAdapter = new MySqlDataAdapter();
        MySqlDataAdapter carsAdapter = new MySqlDataAdapter();
        MySqlDataAdapter orderAdapter = new MySqlDataAdapter();
        MySqlDataAdapter contractAdapter = new MySqlDataAdapter();


        public wolkswagen()
        {
            InitializeComponent();
            initWolkswagenForm();

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.LightGreen700, Primary.LightGreen900, Primary.LightGreen100, Accent.Amber700, TextShade.BLACK);

            auto_panel.Hide();
            customer_panel.Hide();
            staff_panel.Hide();
            contract_panel.Hide();
            complect_panel.Hide();

            back_table_post.Hide();
            panel_add_post.Hide();
            picture_redact_post.Hide();
            picture_post.Hide();
            panel2.Hide();
        }




        /*Загрузка формы*/
        /// <summary>
        /// загрузка формы
        /// </summary>
        public void initWolkswagenForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Текущая дата и время:";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            contract_btn.Items.Add(infoLabel);
            contract_btn.Items.Add(dateLabel);
            contract_btn.Items.Add(timeLabel);
            timer = new System.Windows.Forms.Timer() { Interval = 1000 };
            timer.Tick += timerDatetime_Tick;
            timer.Start();
            colour_box.SelectedItem = "Черный";


        }
        /// <summary>
        /// загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wolkswagen_Load(object sender, EventArgs e)
        {
            connect();

            Font font = new Font("Tahoma", 8);
            contract_table.Font = font;
            staff_table.Font = font;
            post_table.Font = font;

        }
        /// <summary>
        /// отображение текущей даты и времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerDatetime_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }


        /*Подключение к базе данных*/

        /// <summary>
        /// open connection to database
        /// </summary>
        /// <returns></returns>
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server. Contact administrator");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// Close connection
        /// </summary>
        /// <returns></returns>
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public DataTable postDT = new DataTable();
        public DataTable staffDT = new DataTable();
        public DataTable customersDT = new DataTable();
        public DataTable optionsDT = new DataTable();
        public DataTable engineDT = new DataTable();
        public DataTable bodyDT = new DataTable();
        public DataTable manufacturerDT = new DataTable();
        public DataTable equipmentDT = new DataTable();
        public DataTable brandDT = new DataTable();
        public DataTable suuplyDT = new DataTable();
        public DataTable carsDT = new DataTable();
        public DataTable orderDT = new DataTable();
        public DataTable сontracDT = new DataTable();
        /// <summary>
        /// Подключение к базе
        /// </summary>
        private void connect()
        {
            server = "localhost";
            database = "automobile_salon";
            uid = "root";
            password = "Agameb_79";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);

            if (this.OpenConnection() == true)
            {

                contractAdapter = new MySqlDataAdapter("select number_contract AS 'Номер контракта', date_conclusion_contract as 'Дата заключения', type_payment as 'Тип оплаты', maiturity_date as 'Дата погашения', number_staff as 'Сотрудник', number_customer as 'Покупатель', number_order as 'Номер заказа' from contract", connection);
                staffAdapter = new MySqlDataAdapter("select number_staff as'Номер сотрудника', last_name as 'Фамилия', name as 'Имя', middle_name as 'Отчество', address as'Адрес', phone_number as'Телефон', number_post as 'Номер должности' from staffs", connection);
                postAdapter = new MySqlDataAdapter("select number_post as'Номер должности', name as 'Должность' , salary as 'Зарплата' from posts", connection);
                customerAdapter = new MySqlDataAdapter("select number_customer as'Номер покупателя', last_name as 'Фамилия', name as 'Имя', middle_name as 'Отчество', address as 'Адрес', phone_number as 'Телефон' from Customers", connection);
                optionsAdapter = new MySqlDataAdapter("select number_options as'Номер опции', climate_control as'Климат контроль', heated_seats as 'Подогрев сидений' from Options", connection);
                engineAdapter = new MySqlDataAdapter("select number_engine as'Номер двигателя', volume_motor as 'Обьем двигателя', power as 'Мощность', torque as'Крутящий момент' from Engine", connection);
                bodyAdapter = new MySqlDataAdapter("select number_body as'Номер кузова', type_body as'Тип кузова', width as'Ширина', length as'Длина', height as'Высота' from Body", connection);
                manufacturerAdapter = new MySqlDataAdapter("number_manufacturer as'Номер производителя', country_assembly as'Страна сборки', name as'Производитель', address as'Адрес' from Manufacturer", connection);
                equipmentAdapter = new MySqlDataAdapter("select number_equipment as'Номер комплектации', color as'Цвет', weight as'Масса', count_seats as'Колличество мест', number_body as'Номер кузова', number_engine as'Номер двигателя', number_options as'Номер опций' from Equipment", connection);
                brandAdapter = new MySqlDataAdapter("select number_brand as'Номер марки', name as'Марка', number_manufacturer as'Номер производителя', number_equipment as'Номер комплектации' from Brand", connection);
                suuplyAdapter = new MySqlDataAdapter("select code_suuply as'Код поставки', delievery_date as'Дата поставки', count as'Колличество' from Suuply", connection);
                carsAdapter = new MySqlDataAdapter("select number_car as'Номер автомобиля', number_brand as'Номер марки', vin_number as'ВИН номер', code_suuply as'Код поставки', release_year as'Год выпуска', price as'Цена', count as'Колличество' from Cars", connection);
                orderAdapter = new MySqlDataAdapter("select number_order as'Номер заказа', number_car as'Номер автомобиля', count as'Колличество' from Orders", connection);



                contractAdapter.Fill(сontracDT);
                staffAdapter.Fill(staffDT);
                postAdapter.Fill(postDT);
                customerAdapter.Fill(customersDT);
                optionsAdapter.Fill(optionsDT);
                engineAdapter.Fill(engineDT);
                bodyAdapter.Fill(bodyDT);
                // manufacturerAdapter.Fill(manufacturerDT);
                equipmentAdapter.Fill(equipmentDT);
                // brandAdapter.Fill(brandDT);
                // suuplyAdapter.Fill(suuplyDT);
                carsAdapter.Fill(carsDT);
                orderAdapter.Fill(orderDT);



                contractSorce.DataSource = сontracDT;
                staffSorce.DataSource = staffDT;
                postSorce.DataSource = postDT;
                customerSorce.DataSource = customersDT;
                optionsSorce.DataSource = optionsDT;
                engineSorce.DataSource = engineDT;
                bodySorce.DataSource = bodyDT;
                // manufacturerSorce.DataSource = manufacturerDT;
                equipmentSorce.DataSource = equipmentDT;
                //brandSorce.DataSource = brandDT;
                //suuplySorce.DataSource = suuplyDT;
                carstSorce.DataSource = carsDT;
                ordersSorce.DataSource = orderDT;



                contract_table.DataSource = сontracDT;
                staff_table.DataSource = staffDT;
                post_table.DataSource = postDT;
                customer_table.DataSource = customersDT;
                options_table.DataSource = optionsDT;
                engine_table.DataSource = engineDT;
                body_table.DataSource = bodyDT;
                equipment_table.DataSource = equipmentDT;
                order_table.DataSource = orderDT;


                navigator_post.BindingSource = postSorce;
                navigator_contract.BindingSource = contractSorce;
                navigator_customer.BindingSource = customerSorce;
                order_navigator.BindingSource = ordersSorce;
                navigator_staff.BindingSource = staffSorce;
                equipmebt_navigator.BindingSource = equipmentSorce;
                options_navigator.BindingSource = optionsSorce;
                // CloseConnection();
                // this.CloseConnection();
            }


        }


        /*Работа в меню*/

        /// <summary>
        /// меню покупатели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_customer_table_Click(object sender, EventArgs e)
        {
            auto_panel.Show();
            customer_panel.Show();
        }
        private void color_picture_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// меню работники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_staff_table_Click(object sender, EventArgs e)
        {
            auto_panel.Show();
            customer_panel.Show();
            contract_panel.Show();
            staff_panel.Show();


        }
        /// <summary>
        /// Меню автомобили
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_auto_table_Click(object sender, EventArgs e)
        {
            auto_panel.Show();
        }
        /// <summary>
        ///  меню комплектации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_complect_Click(object sender, EventArgs e)
        {
            auto_panel.Show();
            customer_panel.Show();
            contract_panel.Show();
            staff_panel.Show();

            complect_panel.Show();
        }
        /// <summary>
        /// меню поставки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_options_Click(object sender, EventArgs e)
        {
            auto_panel.Show();
            customer_panel.Show();
            contract_panel.Show();
            staff_panel.Show();
        }

        private void colour_box_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void colour_box_TextChanged(object sender, EventArgs e)
        {
            if (colour_box.Text == "Черный")
            {
                FileStream fs = new FileStream(@"d:\octavia_blacks.jpg", FileMode.Open);
                Image img = Image.FromStream(fs);
                fs.Close();
                auto_picture.Image = img;
            }

            if (colour_box.Text == "Белый")
            {
                FileStream fs = new System.IO.FileStream(@"d:\octavia_white.jpg", System.IO.FileMode.Open);
                Image img = Image.FromStream(fs);
                fs.Close();
                auto_picture.Image = img;
            }
            if (colour_box.Text == "Синий")
            {
                FileStream fs = new System.IO.FileStream(@"d:\octavia_bLue.jpg", System.IO.FileMode.Open);
                Image img = Image.FromStream(fs);
                fs.Close();
                auto_picture.Image = img;
            }

        }




        /*Панель автомобили*/

        /// <summary>
        /// назад в меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_auto_Click(object sender, EventArgs e)
        {
            customer_panel.Visible = true;
        }













        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*Панель сотрудники*/

        /// <summary>
        /// Поиск сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_staff_btn_Click(object sender, EventArgs e)
        {
            if (search_staff_box.Text != String.Empty)
            {
                for (int i = 0; i < staff_table.RowCount; i++)
                {
                    for (int j = 0; j < staff_table.ColumnCount; j++)
                    {
                        if (staff_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_staff_box.Text == staff_table.Rows[i].Cells[j].Value.ToString())
                        {
                            staff_table.CurrentCell = staff_table.Rows[i].Cells[j];
                            staff_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }
        /// <summary>
        /// Назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_menu_staff_Click(object sender, EventArgs e)
        {
            staff_panel.Hide();
            contract_panel.Hide();
            customer_panel.Hide();
            auto_panel.Hide();
        }
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_staff_btn_Click(object sender, EventArgs e)
        {
            if (staff_table.Rows.Count > 1)
            {
                int ind = staff_table.CurrentRow.Index;
                staff_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");
            }
        }
        /// <summary>
        /// изиенить - переход в редактирование 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_staff_btn_Click(object sender, EventArgs e)
        {
            tab_control_staffPost.SelectedTab = redact_staff_post_page;
            updateStaf.Show();
            addStaff.Hide();
            panel_add_post.Hide();
            back_table_post.Hide();
            picture_redact_post.Hide();
            picture_post.Hide();

        }
        /// <summary>
        /// Добавление - переход в редактирование
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_staff_btn_Click(object sender, EventArgs e)
        {
            clearStaffs();
            tab_control_staffPost.SelectedTab = redact_staff_post_page;
            updateStaf.Hide();
            addStaff.Show();
            panel_add_post.Hide();
            back_table_post.Hide();
            picture_redact_post.Hide();
            picture_post.Hide();
        }
        /// <summary>
        /// Переход в должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_post_page_Click(object sender, EventArgs e)
        {
            tab_control_staffPost.SelectedTab = postpage;
        }
        /// <summary>
        /// изменить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateStaf_Click(object sender, EventArgs e)
        {
            int i = staff_table.CurrentRow.Index;
            staff_table.Rows[i].Cells[0].Value = number_staff_box.Text;
            staff_table.Rows[i].Cells[1].Value = lastName_staff_box.Text;
            staff_table.Rows[i].Cells[2].Value = name_staff_box.Text;
            staff_table.Rows[i].Cells[3].Value = middle_staff_box.Text;
            staff_table.Rows[i].Cells[4].Value = address_staff_box.Text;
            staff_table.Rows[i].Cells[5].Value = phone_staff_box.Text;
            staff_table.Rows[i].Cells[6].Value = number_postS_box.Text;
        }
        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addStaff_Click(object sender, EventArgs e)
        {
            staff_table.CurrentCell = staff_table.Rows[staff_table.Rows.Count - 1].Cells[0];
            int i = staff_table.CurrentRow.Index;
            staff_table.Rows[i].Cells[0].Value = number_staff_box.Text;
            staff_table.Rows[i].Cells[1].Value = lastName_staff_box.Text;
            staff_table.Rows[i].Cells[2].Value = name_staff_box.Text;
            staff_table.Rows[i].Cells[3].Value = middle_staff_box.Text;
            staff_table.Rows[i].Cells[4].Value = address_staff_box.Text;
            staff_table.Rows[i].Cells[5].Value = phone_staff_box.Text;
            staff_table.Rows[i].Cells[6].Value = number_postS_box.Text;
            tab_control_staffPost.SelectedTab = stafpage;
        }
        /// <summary>
        /// Отображение текущей строки в ткстбоксах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void staff_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            number_staff_box.Text = staff_table.CurrentRow.Cells[0].Value.ToString();
            lastName_staff_box.Text = staff_table.CurrentRow.Cells[1].Value.ToString();
            name_staff_box.Text = staff_table.CurrentRow.Cells[2].Value.ToString();
            middle_staff_box.Text = staff_table.CurrentRow.Cells[3].Value.ToString();
            address_staff_box.Text = staff_table.CurrentRow.Cells[4].Value.ToString();
            phone_staff_box.Text = staff_table.CurrentRow.Cells[5].Value.ToString();
            number_postS_box.Text = staff_table.CurrentRow.Cells[6].Value.ToString();
        }
        /// <summary>
        /// очистка полей
        /// </summary>
        public void clearStaffs()
        {
            number_staff_box.Clear();
            lastName_staff_box.Clear();
            name_staff_box.Clear();
            middle_staff_box.Clear();
            number_postS_box.Clear();
            address_staff_box.Clear();
            phone_staff_box.Clear();
        }

        private void back_table_staff_Click(object sender, EventArgs e)
        {
            tab_control_staffPost.SelectedTab = stafpage;
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*Должности*/
        /// <summary>
        /// переход в сотрудники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_staff_page_Click(object sender, EventArgs e)
        {
            tab_control_staffPost.SelectedTab = stafpage;
        }
        /// <summary>
        /// Поиск должностей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_post_btn_Click(object sender, EventArgs e)
        {
            if (search_post_box.Text != String.Empty)
            {
                for (int i = 0; i < post_table.RowCount; i++)
                {
                    for (int j = 0; j < post_table.ColumnCount; j++)
                    {
                        if (post_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_post_box.Text == post_table.Rows[i].Cells[j].Value.ToString())
                        {
                            post_table.CurrentCell = post_table.Rows[i].Cells[j];
                            post_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_post_btn_Click(object sender, EventArgs e)
        {
            if (post_table.Rows.Count > 1)
            {
                int ind = post_table.CurrentRow.Index;
                post_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");
            }
        }
        /// <summary>
        /// назад в меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_post_btn_Click(object sender, EventArgs e)
        {
            staff_panel.Hide();
            contract_panel.Hide();
            customer_panel.Hide();
            auto_panel.Hide();
        }
        /// <summary>
        /// Добавление - переход в редактирование 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_post_btn_Click(object sender, EventArgs e)
        {
            clearPosts();
            tab_control_staffPost.SelectedTab = redact_staff_post_page;
            panel_add_post.Show();
            back_table_post.Show();
            picture_redact_post.Show();
            picture_post.Show();
            addPost.Show();
            updatePost.Hide();

        }
        /// <summary>
        /// Изменение - переход в редактировании
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_post_box_Click(object sender, EventArgs e)
        {
            tab_control_staffPost.SelectedTab = redact_staff_post_page;
            panel_add_post.Show();
            back_table_post.Show();
            picture_redact_post.Show();
            picture_post.Show();
            addPost.Hide();
            updatePost.Show();


        }
        /// <summary>
        /// Отображение текущей строки в боксах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void post_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            number_post_box.Text = post_table.CurrentRow.Cells[0].Value.ToString();
            post_box.Text = post_table.CurrentRow.Cells[1].Value.ToString();
            salary_box.Text = post_table.CurrentRow.Cells[2].Value.ToString();
        }
        /// <summary>
        /// изменить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatePost_Click(object sender, EventArgs e)
        {
            int i = post_table.CurrentRow.Index;
            post_table.Rows[i].Cells[0].Value = number_postS_box.Text;
            post_table.Rows[i].Cells[1].Value = post_box.Text;
            post_table.Rows[i].Cells[2].Value = salary_box.Text;
            tab_control_staffPost.SelectedTab = postpage;
        }
        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPost_Click(object sender, EventArgs e)
        {
            post_table.CurrentCell = post_table.Rows[post_table.Rows.Count - 1].Cells[0];
            int j = post_table.CurrentRow.Index;
            post_table.Rows[j].Cells[0].Value = number_post_box.Text;
            post_table.Rows[j].Cells[1].Value = post_box.Text;
            post_table.Rows[j].Cells[2].Value = salary_box.Text;
            tab_control_staffPost.SelectedTab = postpage;
        }
        /// <summary>
        /// очистка полей
        /// </summary>
        public void clearPosts()
        {
            number_post_box.Clear();
            post_box.Clear();
            salary_box.Clear();
        }
        private void clear_post_box_Click(object sender, EventArgs e)
        {
            clearPosts();
        }
        /// <summary>
        /// Ограниччения ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void number_staff_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void number_postS_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void phone_staff_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void number_post_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void salary_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }

        /// <summary>
        /// проверки на ввод в таблицу данных
        /// </summary>
        public DataTable changesST = new DataTable();
        public DataTable changesPT = new DataTable();
        private void staff_table_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

            DataTable changesST = ((DataTable)staff_table.DataSource).GetChanges();

            if (changesST != null)
            {
                MySqlCommandBuilder mcbST = new MySqlCommandBuilder(staffAdapter);
                staffAdapter.UpdateCommand = mcbST.GetUpdateCommand();
                staffAdapter.Update(changesST);
                ((DataTable)staff_table.DataSource).AcceptChanges();
            }
        }
        private void post_table_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            DataTable changesPT = ((DataTable)post_table.DataSource).GetChanges();

            if (changesPT != null)
            {
                MySqlCommandBuilder mcbPT = new MySqlCommandBuilder(postAdapter);
                postAdapter.UpdateCommand = mcbPT.GetUpdateCommand();
                postAdapter.Update(changesPT);
                ((DataTable)post_table.DataSource).AcceptChanges();
            }
        }
        private void post_table_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }
        private void staff_table_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }

        private void back_table_post_Click(object sender, EventArgs e)
        {
            tab_control_staffPost.SelectedTab = postpage;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*Панель покупатель*/

        /// <summary>
        /// Назад в меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_customer_Click(object sender, EventArgs e)
        {
            customer_panel.Hide();
            auto_panel.Hide();
        }
        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_customer_btn_Click(object sender, EventArgs e)
        {
            if (search_box.Text != String.Empty)
            {
                for (int i = 0; i < customer_table.RowCount; i++)
                {
                    for (int j = 0; j < customer_table.ColumnCount; j++)
                    {
                        if (customer_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_box.Text == customer_table.Rows[i].Cells[j].Value.ToString())
                        {
                            customer_table.CurrentCell = customer_table.Rows[i].Cells[j];
                            customer_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }
        /// <summary>
        /// Открытие вкладки добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_customer_btn_Click(object sender, EventArgs e)
        {
            clear_customer_boxs();
            tab_control_customer.SelectedTab = redact_customer_page;
            personalCistomer_groupB.Visible = false;
            contactCustomer_groupB.Visible = false;
            adressCustomer_groupB.Visible = false;
            add_customer.Visible = true;
            update_customer.Visible = false;
        }
        /// <summary>
        /// Изменения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_customer_btn_Click(object sender, EventArgs e)
        {
            tab_control_customer.SelectedTab = redact_customer_page;
            personalCistomer_groupB.Visible = false;
            contactCustomer_groupB.Visible = false;
            adressCustomer_groupB.Visible = false;
            add_customer.Visible = false;
            update_customer.Visible = true;

        }
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_customer_btn_Click(object sender, EventArgs e)
        {
            if (customer_table.Rows.Count > 1)
            {
                int ind = customer_table.CurrentRow.Index;
                customer_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");
            }
        }
        /// <summary>
        /// Назад к таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_tc_Click(object sender, EventArgs e)
        {
            tab_control_customer.SelectedTab = tableCustomer_page;
        }

        private void personal_data_customer_CheckedChanged(object sender, EventArgs e)
        {
            if (personal_data_customer.Checked)
            {
                personalCistomer_groupB.Visible = true;
            }
            else
            {
                personalCistomer_groupB.Visible = false;
            }
        }
        private void contact_customer_CheckedChanged(object sender, EventArgs e)
        {
            if (contact_customer.Checked)
            {
                contactCustomer_groupB.Visible = true;
            }
            else
            {
                contactCustomer_groupB.Visible = false;
            }
        }
        private void adresCusstomer_CheckedChanged(object sender, EventArgs e)
        {
            if (adresCusstomer.Checked)
            {
                adressCustomer_groupB.Visible = true;
            }
            else
            {
                adressCustomer_groupB.Visible = false;
            }
        }


        public void clear_customer_boxs()
        {
            lastN_customer_box.Clear();
            name_customer_box.Clear();
            middle_customer_box.Clear();
            city_customer_box.Clear();
            street_customer_box.Clear();
            house_customer_box.Clear();
            apartament_customer_box.Clear();
            phone_customer_box.Clear();
        }
        /// <summary>
        /// Очистка полей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear_pdc_Click(object sender, EventArgs e)
        {
            lastN_customer_box.Clear();
            name_customer_box.Clear();
            middle_customer_box.Clear();
        }
        private void clear_ac_Click(object sender, EventArgs e)
        {

            city_customer_box.Clear();
            street_customer_box.Clear();
            house_customer_box.Clear();
            apartament_customer_box.Clear();
        }
        private void clear_cc_Click(object sender, EventArgs e)
        {
            phone_customer_box.Clear();
        }

        private void add_customer_Click(object sender, EventArgs e)
        {
            customer_table.CurrentCell = customer_table.Rows[customer_table.Rows.Count - 1].Cells[0];
            int i = customer_table.CurrentRow.Index;
            customer_table.Rows[i].Cells[0].Value = number_customer_box.Text;
            customer_table.Rows[i].Cells[1].Value = lastN_customer_box.Text;
            customer_table.Rows[i].Cells[2].Value = name_customer_box.Text;
            customer_table.Rows[i].Cells[3].Value = middle_customer_box.Text;
            customer_table.Rows[i].Cells[4].Value = city_customer_box.Text + " ; " + street_customer_box.Text + " ; " + house_customer_box.Text + " ; " + apartament_customer_box.Text;
            customer_table.Rows[i].Cells[5].Value = phone_customer_box.Text;
            tab_control_customer.SelectedTab = tableCustomer_page;
        }

        /// <summary>
        /// Запрет на ввод букв
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void number_customer_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void phone_customer_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void apartament_customer_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void house_customer_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }

        //доделать
        /// <summary>
        /// Оьображение текущей строки в боксах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customer_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            number_customer_box.Text = customer_table.CurrentRow.Cells[0].Value.ToString();
            lastN_customer_box.Text = customer_table.CurrentRow.Cells[1].Value.ToString();
            name_customer_box.Text = customer_table.CurrentRow.Cells[2].Value.ToString();
            middle_customer_box.Text = customer_table.CurrentRow.Cells[3].Value.ToString();
            city_customer_box.Text = customer_table.CurrentRow.Cells[4].Value.ToString();
            phone_customer_box.Text = customer_table.CurrentRow.Cells[5].Value.ToString();
        }
        /// <summary>
        /// Изменить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_customer_Click(object sender, EventArgs e)
        {
            int i = customer_table.CurrentRow.Index;
            customer_table.Rows[i].Cells[0].Value = number_customer_box.Text;
            customer_table.Rows[i].Cells[1].Value = lastN_customer_box.Text;
            customer_table.Rows[i].Cells[2].Value = name_customer_box.Text;
            customer_table.Rows[i].Cells[3].Value = middle_customer_box.Text;
            customer_table.Rows[i].Cells[4].Value = city_customer_box.Text + " ; " + street_customer_box.Text + " ; " + house_customer_box.Text + " ; " + apartament_customer_box.Text;
            customer_table.Rows[i].Cells[5].Value = phone_customer_box.Text;
            tab_control_customer.SelectedTab = tableCustomer_page;
        }

        public DataTable changeSCT = new DataTable();
        private void customer_table_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }
        private void customer_table_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            /*   DataTable changescCT = ((DataTable)customer_table.DataSource).GetChanges();

               if (changescCT != null)
               {
                   MySqlCommandBuilder mcbCT = new MySqlCommandBuilder(customerAdapter);
                   customerAdapter.UpdateCommand = mcbCT.GetUpdateCommand();
                   customerAdapter.Update(changescCT);
                   ((DataTable)customer_table.DataSource).AcceptChanges();
               }*/
        }

        private void customer_table_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            switch (e.Column.Index)
            {
                case 4:
                    {
                        e.Column.Width += 31;
                        break;
                    }

            }
        }
        private void orders_client_btn_Click(object sender, EventArgs e)
        {

        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*Панель контракт*/

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            Form sForm = new contract();
            sForm.Show();
            //this.Hide();
        }

        /// <summary>
        /// Просмотр панели контракт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_contract_table_Click(object sender, EventArgs e)
        {
            auto_panel.Show();
            customer_panel.Show();
            contract_panel.Show();
        }
        /// <summary>
        /// Назад в меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_co_Click(object sender, EventArgs e)
        {
            contract_panel.Visible = false;
            customer_panel.Visible = false;
            auto_panel.Hide();
        }

        private void add_contract_btn_Click(object sender, EventArgs e)
        {
            clearContracts();
            tab_control_contract.SelectedTab = redact_co_page;
            add_contract.Show();
            updateContract.Hide();
            panel2.Hide();
        }
        /// <summary>
        /// переход в пункт заказы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_orders_page_Click(object sender, EventArgs e)
        {
            tab_control_contract.SelectedTab = order_page;
        }
        /// <summary>
        /// Поиск контракта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_contracc_btn_Click(object sender, EventArgs e)
        {
            if (search_contract_box.Text != String.Empty)
            {
                for (int i = 0; i < contract_table.RowCount; i++)
                {
                    for (int j = 0; j < contract_table.ColumnCount; j++)
                    {
                        if (contract_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_contract_box.Text == contract_table.Rows[i].Cells[j].Value.ToString())
                        {
                            contract_table.CurrentCell = contract_table.Rows[i].Cells[j];
                            contract_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_contract_btn_Click(object sender, EventArgs e)
        {
            if (contract_table.Rows.Count > 1)
            {
                int ind = contract_table.CurrentRow.Index;
                contract_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");
            }
        }
        /// <summary>
        /// переход в изменение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_contract_btn_Click(object sender, EventArgs e)
        {
            tab_control_contract.SelectedTab = redact_co_page;
            add_contract.Hide();
            updateContract.Show();
            panel2.Hide();
        }
        /// <summary>
        /// Отображение в боксах текущей строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contract_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            number_contract_box.Text = contract_table.CurrentRow.Cells[0].Value.ToString();
            date_conclusion_box.Text = contract_table.CurrentRow.Cells[1].Value.ToString();
            type_payment_box.Text = contract_table.CurrentRow.Cells[2].Value.ToString();
            maiturity_date_box.Text = contract_table.CurrentRow.Cells[3].Value.ToString();
            contract_staff_box.Text = contract_table.CurrentRow.Cells[4].Value.ToString();
            contract_customer_box.Text = contract_table.CurrentRow.Cells[5].Value.ToString();
            contract_order_box.Text = contract_table.CurrentRow.Cells[6].Value.ToString();
        }
        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateContract_Click(object sender, EventArgs e)
        {
            int i = contract_table.CurrentRow.Index;
            contract_table.Rows[i].Cells[0].Value = number_contract_box.Text;
            contract_table.Rows[i].Cells[1].Value = date_conclusion_box.Text;
            contract_table.Rows[i].Cells[2].Value = type_payment_box.Text;
            contract_table.Rows[i].Cells[3].Value = maiturity_date_box.Text;
            contract_table.Rows[i].Cells[4].Value = contract_staff_box.Text;
            contract_table.Rows[i].Cells[5].Value = contract_customer_box.Text;
            contract_table.Rows[i].Cells[6].Value = contract_order_box.Text;
            tab_control_contract.SelectedTab = contract_page;
        }
        /// <summary>
        /// Добавить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_contract_Click(object sender, EventArgs e)
        {
            contract_table.CurrentCell = contract_table.Rows[contract_table.Rows.Count - 1].Cells[0];
            int i = contract_table.CurrentRow.Index;
            contract_table.Rows[i].Cells[0].Value = number_contract_box.Text;
            contract_table.Rows[i].Cells[1].Value = date_conclusion_box.Text;
            contract_table.Rows[i].Cells[2].Value = type_payment_box.Text;
            contract_table.Rows[i].Cells[3].Value = maiturity_date_box.Text;
            contract_table.Rows[i].Cells[4].Value = contract_staff_box.Text;
            contract_table.Rows[i].Cells[5].Value = contract_customer_box.Text;
            contract_table.Rows[i].Cells[6].Value = contract_order_box.Text;
            tab_control_contract.SelectedTab = contract_page;
        }

        /// <summary>
        /// Очитска полей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void clearContracts()
        {
            number_contract_box.Clear();
            type_payment_box.Clear();
            contract_customer_box.Clear();
            contract_order_box.Clear();
            contract_staff_box.Clear();
        }
        private void clear_contract_pole_Click(object sender, EventArgs e)
        {
            clearContracts();
        }

        public DataTable changesContractT = new DataTable();
        /// <summary>
        /// Проверки на ввод в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contract_table_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            /*  DataTable changesContractT = ((DataTable)contract_table.DataSource).GetChanges();

           if (changesContractT != null)
           {
               MySqlCommandBuilder mcbConT = new MySqlCommandBuilder(contractAdapter);
               contractAdapter.UpdateCommand = mcbConT.GetUpdateCommand();
               contractAdapter.Update(changesContractT);
               ((DataTable)contract_table.DataSource).AcceptChanges();
           }*/
        }
        private void contract_table_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }
        /// <summary>
        /// Ширина столбцов при добавлении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contract_table_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            switch (e.Column.Index)
            {
                case 6:
                    {
                        e.Column.Width += 3;
                        break;
                    }

            }
        }
        /// <summary>
        /// Запрет на ввод букв
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void number_contract_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void contract_customer_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void contract_staff_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void contract_order_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }

        ///////////////////////////////////////////////////
        /*заказы*/
        /// <summary>
        /// назад в меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_o_Click(object sender, EventArgs e)
        {
            contract_panel.Visible = false;
            customer_panel.Visible = false;
            auto_panel.Hide();
        }
        /// <summary>
        /// перейти к контрактам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_contract_page_Click(object sender, EventArgs e)
        {
            tab_control_contract.SelectedTab = contract_page;
        }
        /// <summary>
        /// Удалить строку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_order_btn_Click(object sender, EventArgs e)
        {
            if (order_table.Rows.Count > 1)
            {
                int ind = order_table.CurrentRow.Index;
                order_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");
            }
        }
        /// <summary>
        /// Добавить заказ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_order_btn_Click(object sender, EventArgs e)
        {
            clearOrder();
            tab_control_contract.SelectedTab = redact_co_page;
            panel2.Show();
            update_order.Hide();
            addOrder.Show();

        }
        /// <summary>
        /// изменить заказ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_order_btn_Click(object sender, EventArgs e)
        {
            tab_control_contract.SelectedTab = redact_co_page;
            panel2.Show();
            update_order.Show();
            addOrder.Hide();

        }
        /// <summary>
        /// поиск заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_order_btn_Click(object sender, EventArgs e)
        {
            if (search_order_box.Text != String.Empty)
            {
                for (int i = 0; i < order_table.RowCount; i++)
                {
                    for (int j = 0; j < order_table.ColumnCount; j++)
                    {
                        if (order_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_order_box.Text == order_table.Rows[i].Cells[j].Value.ToString())
                        {
                            order_table.CurrentCell = order_table.Rows[i].Cells[j];
                            order_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }
        /// <summary>
        /// назад из редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_cont_ord_btn_Click(object sender, EventArgs e)
        {
            tab_control_contract.SelectedTab = contract_page;
        }
        /// <summary>
        /// Ширина столбцов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void order_table_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            switch (e.Column.Index)
            {
                case 0:
                    {
                        e.Column.Width += 28;
                        break;
                    }
                case 1:
                    {
                        e.Column.Width += 28;
                        break;
                    }
                case 2:
                    {
                        e.Column.Width += 28;
                        break;
                    }

            }
        }
        /// <summary>
        /// Отображение в боксах текущей записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void order_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            number_order_box.Text = order_table.CurrentRow.Cells[0].Value.ToString();
            order_auto_box.Text = order_table.CurrentRow.Cells[1].Value.ToString();
            order_count_box.Text = order_table.CurrentRow.Cells[2].Value.ToString();
        }
        /// <summary>
        /// Проверки на ввод в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="anError"></param>
        private void order_table_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }
        public DataTable changesOrderT = new DataTable();
        private void order_table_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            /* DataTable changesOrderT = ((DataTable)order_table.DataSource).GetChanges();

            if (changesOrderT != null)
             {
                MySqlCommandBuilder mcbOrdT = new MySqlCommandBuilder(orderAdapter);
                orderAdapter.UpdateCommand = mcbOrdT.GetUpdateCommand();
                orderAdapter.Update(changesOrderT);
                ((DataTable)order_table.DataSource).AcceptChanges();
            }*/
        }
        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_order_Click(object sender, EventArgs e)
        {
            int i = order_table.CurrentRow.Index;
            order_table.Rows[i].Cells[0].Value = number_order_box.Text;
            order_table.Rows[i].Cells[1].Value = order_auto_box.Text;
            order_table.Rows[i].Cells[2].Value = order_count_box.Text;
            tab_control_contract.SelectedTab = contract_page;
        }
        /// <summary>
        /// Добавление 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addOrder_Click(object sender, EventArgs e)
        {
            order_table.CurrentCell = order_table.Rows[order_table.Rows.Count - 1].Cells[0];
            int i = order_table.CurrentRow.Index;
            order_table.Rows[i].Cells[0].Value = number_order_box.Text;
            order_table.Rows[i].Cells[1].Value = order_auto_box.Text;
            order_table.Rows[i].Cells[2].Value = order_count_box.Text;
            tab_control_contract.SelectedTab = contract_page;
        }
        /// <summary>
        /// Очистка полей
        /// </summary>
        public void clearOrder()
        {
            number_order_box.Clear();
            order_auto_box.Clear();
            order_count_box.Clear();
        }
        private void clear_order_Click(object sender, EventArgs e)
        {
            clearOrder();
        }
        /// <summary>
        /// ограничение ввода букв
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void number_order_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void order_auto_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }
        private void order_count_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Contains('-'))
            {
                if (!((Char.IsDigit(e.KeyChar) && ((TextBox)sender).SelectionStart > 0) || e.KeyChar == (char)Keys.Back))
                    e.Handled = true;
            }
            else if (!(Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || (e.KeyChar == '-' && ((TextBox)sender).SelectionStart == 0)))
                e.Handled = true;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* ПАНЕЛЬ КОМПЛЕКТАЦИЯ  */
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// назад в меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_equip_menu_Click(object sender, EventArgs e)
        {
            complect_panel.Hide();
            staff_panel.Hide();
            contract_panel.Hide();
            customer_panel.Hide();
            auto_panel.Hide();
        }
        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_equip_btn_Click(object sender, EventArgs e)
        {
            if (equipment_table.Rows.Count > 1)
            {
                int ind = equipment_table.CurrentRow.Index;
                equipment_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");
            }
        }
        /// <summary>
        /// Изменить - переход в редакт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_equip_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }
        /// <summary>
        /// Добавить - переход в редакт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_equip_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }
        /// <summary>
        /// переходы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewE_options_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = option_page;
        }
        private void viewE_body_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = body_page;
        }
        private void viewE_engine_btm_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = engine_page;
        }
        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_equip_btn_Click(object sender, EventArgs e)
        {
            if (search_equip_box.Text != String.Empty)
            {
                for (int i = 0; i < equipment_table.RowCount; i++)
                {
                    for (int j = 0; j < equipment_table.ColumnCount; j++)
                    {
                        if (equipment_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_equip_box.Text == equipment_table.Rows[i].Cells[j].Value.ToString())
                        {
                            equipment_table.CurrentCell = equipment_table.Rows[i].Cells[j];
                            equipment_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }

        private void equipment_table_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void equipment_table_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void equipment_table_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*Опции*/

        private void back_option_menu_Click(object sender, EventArgs e)
        {
            complect_panel.Hide();
            staff_panel.Hide();
            contract_panel.Hide();
            customer_panel.Hide();
            auto_panel.Hide();
        }

        private void delete_option_btn_Click(object sender, EventArgs e)
        {
            if (options_table.Rows.Count > 1)
            {
                int ind = options_table.CurrentRow.Index;
                options_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");
            }
        }

        private void update_option_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }

        private void add_option_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }

        private void viewO_equip_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = equipment_page;
        }
        private void viewO_body_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = body_page;
        }
        private void viewO_engine_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = engine_page;
        }
        /// <summary>
        /// поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_option_btn_Click(object sender, EventArgs e)
        {
            if (search_option_box.Text != String.Empty)
            {
                for (int i = 0; i < options_table.RowCount; i++)
                {
                    for (int j = 0; j < options_table.ColumnCount; j++)
                    {
                        if (options_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_option_box.Text == options_table.Rows[i].Cells[j].Value.ToString())
                        {
                            options_table.CurrentCell = options_table.Rows[i].Cells[j];
                            options_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }

        private void options_table_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void options_table_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*Двигатель*/
        /// <summary>
        /// Назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_engine_menu_Click(object sender, EventArgs e)
        {
            complect_panel.Hide();
            staff_panel.Hide();
            contract_panel.Hide();
            customer_panel.Hide();
            auto_panel.Hide();
        }
        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_engine_btn_Click(object sender, EventArgs e)
        {
            if (engine_table.Rows.Count > 1)
            {
                int ind = engine_table.CurrentRow.Index;
                engine_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");

            }

        }
        /// <summary>
        /// Обновить запись переход в редактирование
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_engine_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }
        /// <summary>
        /// Добавить запись - переход в редактирование
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_engine_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }
        /// <summary>
        /// переход по записям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewEn_options_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = option_page;
        }
        private void viewEn_body_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = body_page;
        }
        private void viewEn_equipment_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = equipment_page;
        }
        /// <summary>
        /// поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_engine_btn_Click(object sender, EventArgs e)
        {
            if (search_engine_box.Text != String.Empty)
            {
                for (int i = 0; i < engine_table.RowCount; i++)
                {
                    for (int j = 0; j < engine_table.ColumnCount; j++)
                    {
                        if (engine_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_engine_box.Text == engine_table.Rows[i].Cells[j].Value.ToString())
                        {
                            engine_table.CurrentCell = engine_table.Rows[i].Cells[j];
                            engine_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*Кузов*/
        /// <summary>
        /// Назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_body_menu_Click(object sender, EventArgs e)
        {
            complect_panel.Hide();
            staff_panel.Hide();
            contract_panel.Hide();
            customer_panel.Hide();
            auto_panel.Hide();
                
        }
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_body_btn_Click(object sender, EventArgs e)
        {
            if (body_table.Rows.Count > 1)
            {
                int ind = body_table.CurrentRow.Index;
                body_table.Rows.RemoveAt(ind);
            }
            else
            {
                MessageBox.Show("Удаление невозможно");

            }
        }
        /// <summary>
        /// изменение - переходв в редактированиеы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_body_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }
        /// <summary>
        /// Добавление - переход в редактирование
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_body_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = redact_eoeb_page;
        }
        /// <summary>
        /// переходы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewB_options_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = option_page;
        }
        private void viewB_equip_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = equipment_page;
        }
        private void view_engine_btn_Click(object sender, EventArgs e)
        {
            tab_control_equip.SelectedTab = engine_page;
        }
        /// <summary>
        /// поиск
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_body_btn_Click(object sender, EventArgs e)
        {
            if (search_body_box.Text != String.Empty)
            {
                for (int i = 0; i < body_table.RowCount; i++)
                {
                    for (int j = 0; j < body_table.ColumnCount; j++)
                    {
                        if (body_table.Rows[i].Cells[j].Value == null)
                        {
                            /*message = "NULL";
                            Form sForm = new info(message);
                            sForm.Show();*/
                            break;
                        }
                        if (search_body_box.Text == body_table.Rows[i].Cells[j].Value.ToString())
                        {
                            body_table.CurrentCell = body_table.Rows[i].Cells[j];
                            body_table.FirstDisplayedScrollingRowIndex = i;
                            break;

                        }

                    }

                }
            }
        }

      
    }
}
