using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HoW_III
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private MySqlConnectionStringBuilder conexaoBanco()
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "reparos";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            conexaoBD.SslMode = 0;
            return conexaoBD;
        }

        private void atualizarGrid()
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "SELECT * FROM agenda";
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dgAgenda.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dgAgenda.Rows[0].Clone();
                    row.Cells[0].Value = reader.GetInt32(0);    //Id
                    row.Cells[1].Value = reader.GetString(1);   //Nome
                    row.Cells[2].Value = reader.GetString(2);   //Telefone
                    row.Cells[3].Value = reader.GetString(3);   //Hora
                    row.Cells[4].Value = reader.GetString(4);   //Data
                    row.Cells[5].Value = reader.GetString(5);   //Endereço
                    dgAgenda.Rows.Add(row);                     
                }

                realizaConexacoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível conectar-se ao banco de dados.");
                Console.WriteLine(ex.Message);
            }
        }

        private void limparCampos()
        {
            tbId.Clear();
            tbNome.Clear();
            tbTelefone.Clear();
            tbHora.Clear();
            tbData.Clear();
            tbEndereco.Clear();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            atualizarGrid();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "INSERT INTO agenda (nomeCliente,telefoneCliente,horaReparo,dataReparo,enderecoCliente) " +
                    "VALUES('" + tbNome.Text + "', '" + tbTelefone.Text + "','" + tbHora.Text + "','" + tbData.Text + "','" + tbEndereco.Text + "')";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close();
                MessageBox.Show("Reparo do(a) " + tbNome.Text + "[" + tbTelefone.Text + "] foi agendado para às " + tbHora.Text + " do dia " + tbData.Text + ".");
                atualizarGrid();
                limparCampos();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void calendario_DateChanged(object sender, DateRangeEventArgs e)
        {
            tbData.Text = calendario.SelectionRange.Start.ToShortDateString();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); 
                comandoMySql.CommandText = "UPDATE agenda SET nomeCliente= '" + tbNome.Text + "', " +
                    "telefoneCliente = '" + tbTelefone.Text + "', " +
                    "horaReparo = '" + tbHora.Text + "', " +
                    "dataReparo = '" + tbData.Text + "', " +
                    "enderecoCliente = '" + tbEndereco.Text + "' " +
                    " WHERE idCliente = " + tbId.Text + "";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); 
                MessageBox.Show("ALTERADO com sucesso!"); 
                atualizarGrid();
                limparCampos();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); 
                comandoMySql.CommandText = "DELETE FROM agenda WHERE idCliente = " + tbId.Text + "";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); 
                MessageBox.Show("ID " + tbId.Text + "(" + tbNome.Text + ", " + tbTelefone.Text + ", " + tbHora.Text + " - " + tbData.Text + ")" + " EXCLUÍDO com sucesso!");
                atualizarGrid();
                limparCampos();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void dgAgenda_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgAgenda.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgAgenda.CurrentRow.Selected = true;
                tbId.Text = dgAgenda.Rows[e.RowIndex].Cells["ColumnId"].FormattedValue.ToString();
                tbNome.Text = dgAgenda.Rows[e.RowIndex].Cells["ColumnNome"].FormattedValue.ToString();
                tbTelefone.Text = dgAgenda.Rows[e.RowIndex].Cells["ColumnTelefone"].FormattedValue.ToString();
                tbHora.Text = dgAgenda.Rows[e.RowIndex].Cells["ColumnHora"].FormattedValue.ToString();
                tbData.Text = dgAgenda.Rows[e.RowIndex].Cells["ColumnData"].FormattedValue.ToString();
                tbEndereco.Text = dgAgenda.Rows[e.RowIndex].Cells["ColumnEndereco"].FormattedValue.ToString();
            }
        }
    }
}
