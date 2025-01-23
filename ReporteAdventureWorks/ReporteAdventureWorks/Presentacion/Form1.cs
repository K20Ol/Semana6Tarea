using ReporteAdventureWorks.Config;
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

namespace ReporteAdventureWorks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conexion con = new conexion();
            using (SqlConnection connection = con.obtenerConexion())
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT TABLE_SCHEMA + '.' + TABLE_NAME AS FullTableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cmbTablas.Items.Add(reader["FullTableName"].ToString());
                }
            }
            this.reportViewer1.RefreshReport();
        }
        private DataTable GetData(string query)
        {
            conexion con = new conexion();
            using (SqlConnection connection = con.obtenerConexion())
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        private void ShowReport(DataTable dataTable, string tableName)
        {
            reportViewer1.Reset();
            reportViewer1.LocalReport.ReportEmbeddedResource = "ReporteAdventureWorks.Reporte.rdlc"; 

            // Asignar el DataSource al ReportViewer
            Microsoft.Reporting.WinForms.ReportDataSource rds = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dataTable);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Parámetro opcional para mostrar el nombre de la tabla en el reporte
          

            reportViewer1.RefreshReport();
        }
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (cmbTablas.SelectedItem != null)
            {
                string selectedTable = cmbTablas.SelectedItem.ToString();
                string query = $"SELECT * FROM {selectedTable}";

                DataTable dataTable = GetData(query);
                ShowReport(dataTable, selectedTable);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una tabla.");
            }
        }
    }
}
