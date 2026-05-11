using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;

namespace TourGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Versenyzo> adatok = [];
        public string connectionString = "Filename=tour.db";
        public SqliteConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            connection = new(connectionString);
            connection.Open();
            string lekerdez = """
                SELECT versenyzo.id,nev,csapatNev,nemzetiseg
                FROM csapat INNER JOIN versenyzo ON csapat.id=versenyzo.csapatId
                """;
            SqliteCommand command = new(lekerdez, connection);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nev = reader.GetString(1);
                string csapatNev = reader.GetString(2);
                string nemzetiseg = reader.GetString(3);
                adatok.Add(new(id, nev, csapatNev, nemzetiseg));
            }
            reader.Close();
            resultTable.ItemsSource = adatok;
            resultTable.SelectedIndex = 0;
        }

        private void resultTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Versenyzo valasztott = resultTable.SelectedItem as Versenyzo;
            string lekerdez = $"""
                SELECT ido
                FROM versenyzo INNER JOIN eredmeny ON versenyzo.id=eredmeny.versenyzoId
                WHERE szakasz=5 AND versenyzo.id='{valasztott.Id}'
                """;
            SqliteCommand command = new(lekerdez, connection);
            SqliteDataReader reader = command.ExecuteReader();
            reader.Read();
            feladat2Label.Content = $"5. szakasz eredménye: {reader.GetString(0)}";
            reader.Close();
        }

        private void feladat3Button_Click(object sender, RoutedEventArgs e)
        {
            string lekerdez = """
                SELECT csapatNev,COUNT(*)
                FROM csapat INNER JOIN versenyzo ON csapat.id=versenyzo.csapatId
                GROUP BY csapatNev
                """;
            SqliteCommand command = new(lekerdez, connection);
            SqliteDataReader reader = command.ExecuteReader();
            List<string> tarolo = [];
            while(reader.Read())
            {
                tarolo.Add($"{reader.GetString(0)}: {reader.GetInt32(1)} fő");
            }
            feladat3List.ItemsSource = tarolo;
            reader.Close();
        }

        private void feladat4Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Magyar versenyzők száma: {adatok.Count(x => x.Nemzetiseg == "HUN")}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourGUI
{
    class Versenyzo
    {
        public Versenyzo(int id, string nev, string csapatNev, string nemzetiseg)
        {
            Id = id;
            Nev = nev;
            CsapatNev = csapatNev;
            Nemzetiseg = nemzetiseg;
        }

        public int Id { get; set; }
        public string Nev { get; set; }
        public string CsapatNev { get; set; }
        public string Nemzetiseg { get; set; }
    }
}

<Window x:Class="TourGUI.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:TourGUI"
        mc:Ignorable="d"
        Title="TourGUI" Height="450" Width="800">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="2*"/>
            <ColumnDefinition Width="1*"/>
        </Grid.ColumnDefinitions>
        <DataGrid x:Name="resultTable" d:ItemsSource="{d:SampleData ItemCount=5}" SelectionChanged="resultTable_SelectionChanged"/>
        <StackPanel Grid.Column="1">
            <Label x:Name="feladat2Label" Content="5. szakasz eredménye: "/>
            <Button x:Name="feladat3Button" Content="Csapatonkénti versenyzők száma" Click="feladat3Button_Click"/>
            <ListBox x:Name="feladat3List" Height="100" d:ItemsSource="{d:SampleData ItemCount=5}"/>
            <Button x:Name="feladat4Button" Content="Magyar versenyzők száma" Click="feladat4Button_Click"/>
        </StackPanel>

    </Grid>
</Window>

