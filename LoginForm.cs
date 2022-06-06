using System.Data;
using System.Data.SqlClient;

namespace Authorization
{
    public partial class LoginForm : Form
    {
        public static Form myForm;
        public static string PathToProgram;

        public LoginForm()
        {
            InitializeComponent();
            myForm = this;
            Size = new Size(700, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "Авторизация";
            PathToProgram = string.Join('\\',Directory.GetCurrentDirectory().Split('\\').SkipLast(3));

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
                Text = "Авторизация\r\n(Введите логин и пароль)",
                TextAlign = HorizontalAlignment.Center,
                Font = new Font("Times New Roman", 16),
                BorderStyle = BorderStyle.None,
                BackColor = this.BackColor,
            };


            var boxSize = new Size(400, 35);
            var loginBox = new TextBox()
            {
                Size = boxSize,
                Location = new Point(Width / 2 - boxSize.Width / 2, Height / 2-boxSize.Height),
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
                Location = new Point(passwordBox.Location.X + passwordBox.Width/2 - readyButtonSize.Width/2
                , passwordBox.Location.Y+passwordBox.Height+20),
                Size = readyButtonSize,
                Text = "Готово",
            };
            readyButton.Click += AuthorizeUser(loginBox,passwordBox);

            var link = new LinkLabel()
            {
                Location = new Point(readyButton.Location.X, readyButton.Location.Y+readyButton.Height),
                Size = new Size(150, 75),
                Text = "Ещё нет аккаунта? Регистрация.",
                TextAlign = ContentAlignment.MiddleCenter,
            };
            link.Click += (s, e) => {
                if (RegistrationForm.myForm == null)
                    new RegistrationForm().Show();
                else
                    RegistrationForm.myForm.Show();
                this.Hide(); 
            };

            Controls.Add(passwordBox);
            Controls.Add(loginBox);
            Controls.Add(description);
            Controls.Add(readyButton);
            Controls.Add(link);
        }

        private EventHandler AuthorizeUser(TextBox loginBox, TextBox passwordBox)
        {
            return (s, e) =>
            {
                var login = loginBox.Text;
                var password = passwordBox.Text;

                var database = new DataBase(PathToProgram + @"\accountData.MDF");

                database.Open();

                var adapter = new SqlDataAdapter();
                var table = new DataTable();
                var query = $@"select userLogin, userPassword from registration where userLogin = '{login}' and userPassword = '{password}'";
                var command = new SqlCommand(query, database.Connection);

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count == 1)
                    MessageBox.Show("Вы успешно вошли в систему.");
                else
                    MessageBox.Show("Неправильный логин или пароль.");

                database.Close();
            };
        }
    }
}