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
        private readonly conexion conexion = new conexion();
        public Form1()
        {
            InitializeComponent(); CargarTablas();
        }
        private void CargarTablas()
        {
            try
            {
                using (SqlConnection connection = conexion.obtenerConexion())
                {
                    connection.Open();
                    string query = "SELECT TABLE_SCHEMA + '.' + TABLE_NAME AS FullTableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbTablas.Items.Add(reader["FullTableName"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las tablas: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void GenerarReporte(string nombreTabla)
        {
            try
            {
                using (SqlConnection connection = conexion.obtenerConexion())
                {
                    connection.Open();

                    
                    string query = $"SELECT * FROM [{nombreTabla.Replace(".", "].[")}]";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                     
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            dataTable.Columns[i].ColumnName = "DataColumn" + (i + 1);
                        }

                    
                        reportViewer1.LocalReport.DataSources.Clear();

                       
                        Microsoft.Reporting.WinForms.ReportDataSource dataSource = new Microsoft.Reporting.WinForms.ReportDataSource
                        {
                            Name = "DataSet1", 
                            Value = dataTable
                        };
                        reportViewer1.LocalReport.DataSources.Add(dataSource);

                        
                        reportViewer1.RefreshReport();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el reporte: " + ex.Message);
            }
        }
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (cmbTablas.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una tabla para generar el reporte.");
                return;
            }

            string tablaSeleccionada = cmbTablas.SelectedItem.ToString();
            GenerarReporte(tablaSeleccionada);
        }
    }
}
