using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Authorization
{
    public partial class RegistrationForm : Form
    {
        public static Form myForm;
        public RegistrationForm()
        {
            InitializeComponent();
            myForm = this;
            Size = new Size(700, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "Регистрация";

            FormClosed += (s, e) => { LoginForm.myForm.Close(); };

            AddComponentsToForm();
        }

        private void AddComponentsToForm()
        {
            var textSize = new Size(400, 100);
            var description = new TextBox()
            {
                Size = textSize,
                Location = new Point(Width / 2 - textSize.Width / 2, 0),
                Multiline = true,
                Text = "Регистрация",
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Times New Roman", 16),
                BorderStyle = BorderStyle.None,
                BackColor = this.BackColor,
            };


            var boxSize = new Size(400, 35);
            var loginBox = new TextBox()
            {
                Size = boxSize,
                Location = new Point(Width / 2 - boxSize.Width / 2, Height / 2 - boxSize.Height),
                TextAlign = HorizontalAlignment.Center,
                Text = "Логин",
                Font = new Font("Times New Roman", 16),
                ForeColor = Color.Silver,
            };
            var passwordBox = new TextBox()
            {
                Size = boxSize,
                Location = new Point(Width / 2 - boxSize.Width / 2, Height / 2 + boxSize.Height),
                TextAlign = HorizontalAlignment.Center,
                Text = "Пароль",
                Font = new Font("Times New Roman", 16),
                ForeColor = Color.Silver,
            };

            loginBox.Enter += (s, e) => {
                if (loginBox.Text == "Логин")
                {
                    loginBox.Text = "";
                    loginBox.ForeColor = Color.Black;
                }
            };
            loginBox.Leave += (s, e) => {
                if (loginBox.Text == "")
                {
                    loginBox.Text = "Логин";
                    loginBox.ForeColor = Color.Silver;
                }
            };

            passwordBox.Enter += (s, e) => {
                if (passwordBox.Text == "Пароль")
                {
                    passwordBox.Text = "";
                    passwordBox.ForeColor = Color.Black;
                }
            };
            passwordBox.Leave += (s, e) => {
                if (passwordBox.Text == "")
                {
                    passwordBox.Text = "Пароль";
                    passwordBox.ForeColor = Color.Silver;
                }
            };

            var readyButtonSize = new Size(150, 35);
            var readyButton = new Button()
            {
                Location = new Point(passwordBox.Location.X + passwordBox.Width / 2 - readyButtonSize.Width / 2
                , passwordBox.Location.Y + passwordBox.Height + 20),
                Size = readyButtonSize,
                Text = "Готово",
            };
            readyButton.Click += RegistrateUser(loginBox, passwordBox);

            var link = new LinkLabel()
            {
                Location = new Point(readyButton.Location.X, readyButton.Location.Y + readyButton.Height),
                Size = new Size(150, 75),
                Text = "Назад",
                TextAlign = ContentAlignment.MiddleCenter,
            };
            link.Click += (s, e) => { LoginForm.myForm.Show(); this.Hide(); };

            Controls.Add(passwordBox);
            Controls.Add(loginBox);
            Controls.Add(description);
            Controls.Add(readyButton);
            Controls.Add(link);
        }

        private EventHandler RegistrateUser(TextBox loginBox, TextBox passwordBox)
        {
            return (s, e) =>
            {
                var login = loginBox.Text;
                var password = passwordBox.Text;
                if (login == null || login.Length<1 || login == "Логин"
                || password == null || password.Length <1 || password == "Пароль")
                {
                    MessageBox.Show("Некорректные логин или пароль");
                    return;
                }

                var database = new DataBase(LoginForm.PathToProgram + @"\accountData.MDF");
                database.Open();

                var adapter = new SqlDataAdapter();
                var table = new DataTable();
                var query = $@"select userLogin, userPassword from registration where userLogin = '{login}'";
                var command = new SqlCommand(query, database.Connection);
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count>0)
                {
                    MessageBox.Show("Аккаунт с таким логином уже зарегистрирован.");
                    return;
                }

                query = $@"insert into registration(userLogin, userPassword) values ('{login}', '{password}')";
                command = new SqlCommand(query, database.Connection);
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Аккаунт успешно создан.");
                    LoginForm.myForm.Show();
                    this.Hide();
                }

                database.Close();
            };
        }
    }
}
