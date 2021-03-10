using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieFile
{
    public partial class Form1 : Form
    {

        private string queryString = @"Data Source=DESKTOP-FTT4EST\SQLEXPRESS;Initial Catalog=ATMovies3;Integrated Security=True";
        SqlConnection con;
        SqlDataAdapter adptr;
        DataSet dsMovies;
        DataTable dtMovies;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbCategoryInsert.SelectedItem = cbCategoryUpdate.Items[0];
            cbCategoryUpdate.SelectedItem = cbCategoryUpdate.Items[0];
            pnlInsertMovie.BringToFront();
            dsMovies = new DataSet();
            con = new SqlConnection(queryString);
            adptr = new SqlDataAdapter("SelectMoviesTable", con);
            adptr.SelectCommand.CommandType = CommandType.StoredProcedure;
            RenderGrid();
        }

        private void RenderGrid()
        {
            adptr.Fill(dsMovies, "Movies");
            dtMovies = dsMovies.Tables["Movies"];
            dgvMovies.DataSource = dtMovies;
        }

        private void btnInsertMovie_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMovieIdInsert.Text.Equals("") || txtMovieTitleInsert.Text.Equals("") || cbCategoryInsert.Text.Equals(""))
                {
                    MessageBox.Show("All the fields must to be filled.");
                    return;
                }
                foreach (DataRow row in dtMovies.Rows)
                {
                    if (row.RowState != DataRowState.Deleted && row["ID"].ToString() == txtMovieIdInsert.Text)
                    {
                        MessageBox.Show("The id is already exist!");
                        return;
                    }
                }
                DataRow dr = dtMovies.NewRow();
                dr["id"] = txtMovieIdInsert.Text;
                dr["title"] = txtMovieTitleInsert.Text;
                dr["category"] = cbCategoryInsert.Text;
                dr["release_date"] = dtpMovieReleasedInsert.Value.ToShortDateString();
                dtMovies.Rows.Add(dr);
                RenderGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateMovie_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMovieIdUpdate.Text.Equals("") || txtMovieTitleUpdate.Text.Equals("") || cbCategoryUpdate.Text.Equals(""))
                {
                    MessageBox.Show("All the fields must to be filled.");
                    return;
                }
                foreach (DataRow row in dtMovies.Rows)
                {
                    if (row.RowState != DataRowState.Deleted && row["id"].ToString() == txtMovieIdUpdate.Text)
                    {
                        row["title"] = txtMovieTitleUpdate.Text;
                        row["category"] = cbCategoryUpdate.Text;
                        row["release_date"] = dtpMovieReleasedUpdate.Value.ToShortDateString();
                    }
                }
                MessageBox.Show("The movie has been updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteMovie_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMovieIdDelete.Text.Equals("") || txtMovieTitleDelete.Text.Equals("") || cbCategoryDelete.Text.Equals(""))
                {
                    MessageBox.Show("All the fields must to be filled.");
                    return;
                }
                DialogResult result;
                //Message box with yes / no 
                result = MessageBox.Show($"Are you sure that you want to delete Movie ID: {txtMovieIdDelete.Text}", "Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result.Equals(DialogResult.Yes))
                {
                    foreach (DataRow row in dtMovies.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted && row["id"].ToString() == txtMovieIdDelete.Text)
                        {
                            row.Delete();
                            break;
                        }
                    }
                    txtMovieIdDelete.Clear();
                    txtMovieTitleDelete.Clear();
                    cbCategoryDelete.Text = "";
                    dtpMovieReleasedDelete.Value = DateTime.Now;
                    MessageBox.Show("Delete succeed.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dgvMovies_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvMovies.SelectedRows)
            {
                txtMovieIdUpdate.Text = row.Cells[0].Value.ToString();
                txtMovieTitleUpdate.Text = row.Cells[1].Value.ToString();
                txtMovieIdDelete.Text = row.Cells[0].Value.ToString();
                txtMovieTitleDelete.Text = row.Cells[1].Value.ToString();
                string category = row.Cells[2].Value.ToString();
                if (cbCategoryUpdate.Items[0].ToString().Equals(category))
                {
                    cbCategoryUpdate.SelectedItem = cbCategoryUpdate.Items[0];
                }
                if (cbCategoryUpdate.Items[1].ToString().Equals(category))
                {
                    cbCategoryUpdate.SelectedItem = cbCategoryUpdate.Items[1];
                }
                if (cbCategoryUpdate.Items[2].ToString().Equals(category))
                {
                    cbCategoryUpdate.SelectedItem = cbCategoryUpdate.Items[2];
                }

                if (cbCategoryDelete.Items[0].ToString().Equals(category))
                {
                    cbCategoryDelete.SelectedItem = cbCategoryUpdate.Items[0];
                }
                if (cbCategoryDelete.Items[1].ToString().Equals(category))
                {
                    cbCategoryDelete.SelectedItem = cbCategoryUpdate.Items[1];
                }
                if (cbCategoryDelete.Items[2].ToString().Equals(category))
                {
                    cbCategoryDelete.SelectedItem = cbCategoryUpdate.Items[2];
                }

                string date = row.Cells[3].Value.ToString();
                string[] dateArray = date.Split('/');
                dtpMovieReleasedUpdate.Value = new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[1]), int.Parse(dateArray[0]));
                dtpMovieReleasedDelete.Value = new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[1]), int.Parse(dateArray[0]));

            }
        }

        private void btnInsertMovieView_Click(object sender, EventArgs e)
        {
            pnlInsertMovie.BringToFront();
        }

        private void btnUpdateMovieView_Click(object sender, EventArgs e)
        {
            pnlUpdateMovie.BringToFront();
        }
        private void btnDeleteMovieView_Click(object sender, EventArgs e)
        {
            pnlDeleteMovie.BringToFront();
        }

        private void btnUpdateDB_Click(object sender, EventArgs e)
        {
            new SqlCommandBuilder(adptr);
            adptr.Update(dtMovies);
            dsMovies = new DataSet();
            RenderGrid();
        }
    }
}
