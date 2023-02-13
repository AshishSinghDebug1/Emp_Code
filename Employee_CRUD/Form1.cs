using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft;

using static Employee_CRUD.Program;

namespace Employee_CRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            rbmale.Checked = true;
            rbactive.Checked = true;
            var dataTable = GetInfo("");
            dataGridView1.DataSource = await dataTable;
            btnsubmit.Text = "Submit";
        }

        public static async Task<DataTable> GetInfo(string requestParams)
        {
            // Initialization.  
            DataTable responseObj = new DataTable();

            // HTTP GET.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("https://gorest.co.in/");

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP GET  
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");
              
              response = await client.GetAsync("public/v2/users").ConfigureAwait(false);                

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                        string result = response.Content.ReadAsStringAsync().Result;
                    responseObj = JsonConvert.DeserializeObject<DataTable>(result);
                }
            }
            return responseObj;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtname.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtemail.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().ToUpper() == "MALE")
                rbmale.Checked = true;
            else
                rbfemale.Checked = true;

            if (dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().ToUpper() == "ACTIVE")
                rbactive.Checked = true;
            else
                rbinactive.Checked = true;
            btnsubmit.Text = "Update";

        }

        private async void btnsubmit_Click(object sender, EventArgs e)
        {
            if (txtId.Text != string.Empty)
            {
                //Update
                string gender;
                string status;
                if (rbmale.Checked)
                {
                    gender = "Male";
                }
                else
                {
                    gender = "Female";
                }
                if (rbactive.Checked)
                {
                    status = "Active";
                }
                else
                {
                    status = "Inactive";
                }

                DataSet ds = new DataSet();
                DataTable responseObj = new DataTable();
                var token = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";
                using (var client = new HttpClient())
                {
                    var jsoncontent = new StringContent(JsonConvert.SerializeObject(new
                    { name = txtname.Text, email = txtemail.Text, gender = gender, status = status }),
                        Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
                    var result = await client.PutAsync("https://gorest.co.in/public/v2/users/" + txtId.Text, jsoncontent);
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new ArgumentException("failed");
                    }
                    var jsonresult = await result.Content.ReadAsStringAsync();
                    // responseObj = JsonConvert.DeserializeObject<DataTable>(jsonresult);

                    Employee employee = JsonConvert.DeserializeObject<Employee>(jsonresult);
                    if (employee.id > 0)
                    {
                        MessageBox.Show("Update Succesfully");
                        var dataTable = GetInfo("");
                        dataGridView1.DataSource = await dataTable;
                        btnsubmit.Text = "Submit";
                    }
                    else
                    {
                        MessageBox.Show("Not Updated ");
                    }
                }
            }
            else
            {
                //Insert
                string gender;
                string status;
                if (rbmale.Checked)
                {
                    gender = "Male";
                }
                else
                {
                    gender = "Female";
                }
                if (rbactive.Checked)
                {
                    status = "Active";
                }
                else
                {
                    status = "Inactive";
                }

                DataSet ds = new DataSet();
                DataTable responseObj = new DataTable();
                var token = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";
                using (var client = new HttpClient())
                {
                    var jsoncontent = new StringContent(JsonConvert.SerializeObject(new
                    { name = txtname.Text, email = txtemail.Text, gender = gender, status = status }),
                        Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
                    var result = await client.PostAsync("https://gorest.co.in/public/v2/users", jsoncontent);
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new ArgumentException("failed");
                    }
                    var jsonresult = await result.Content.ReadAsStringAsync();
                    // responseObj = JsonConvert.DeserializeObject<DataTable>(jsonresult);

                    Employee employee = JsonConvert.DeserializeObject<Employee>(jsonresult);
                    if (employee.id > 0)
                    {
                        MessageBox.Show("Insert Succesfully");
                        var dataTable = GetInfo("");
                        dataGridView1.DataSource = await dataTable;
                        btnsubmit.Text = "Submit";
                    }
                    else
                    {
                        MessageBox.Show("Not Insert ");
                    }
                }

            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            txtname.Text = "";
            txtemail.Text = "";
            txtId.Text = "";
            rbmale.Checked = true;
            rbactive.Checked = true;
        }

        private async void btndelete_Click(object sender, EventArgs e)
        {


            DataSet ds = new DataSet();
            DataTable responseObj = new DataTable();
            var token = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
                var result = await client.DeleteAsync("https://gorest.co.in/public/v2/users/" + txtId.Text);
                if (!result.IsSuccessStatusCode)
                {
                    throw new ArgumentException("failed");
                }
                var jsonresult = await result.Content.ReadAsStringAsync();
                // responseObj = JsonConvert.DeserializeObject<DataTable>(jsonresult);


                if (jsonresult== string.Empty)
                {
                    MessageBox.Show("Delete Succesfully");
                    var dataTable = GetInfo("");
                    dataGridView1.DataSource = await dataTable;
                    btnsubmit.Text = "Submit";
                    txtname.Text = "";
                    txtemail.Text = "";
                    txtId.Text = "";
                    rbmale.Checked = true;
                    rbactive.Checked = true;
                }
                else
                {
                    MessageBox.Show("Not Delete ");
                }

            }

        }

       
    }
}
