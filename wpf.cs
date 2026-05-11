MAINWINDOW.XAML.CS ----------------------------------------------------------------->>>
  
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using Dapper;

namespace TourGUI {
    public partial class MainWindow : Window {
        List<Versenyzo> versenyzok = new List<Versenyzo>();
        string connectionString = "Data Source=tour.sql;Version=3;";

        public MainWindow() {
            InitializeComponent();
            AdatokBetoltese();
            compDisplay.ItemsSource = versenyzok;
            compDisplay.SelectedIndex = 0;
        }

        void AdatokBetoltese() {
            using var conn = new SQLiteConnection(connectionString);
            versenyzok = conn.Query<Versenyzo>(
                "SELECT v.id, v.nev, v.nemzetiseg, c.csapatNev FROM versenyzo v JOIN csapat c ON v.csapatId = c.id"
            ).ToList();
        }

        void selectionChanged(object sender, SelectionChangedEventArgs e) {
            if (compDisplay.SelectedItem is Versenyzo v){
                using var conn = new SQLiteConnection(connectionString);
                var ido = conn.ExecuteScalar<string>("SELECT ido FROM eredmeny WHERE versenyzoId = @Id AND szakasz = 5", new { v.Id });
                resultsText.Text = $"5. szakasz eredménye: {ido ?? "Nincs adat"}";
            }
        }

        void compsCount(object sender, RoutedEventArgs e) {
            teamList.ItemsSource = versenyzok.GroupBy(v => v.CsapatNev)
                                             .Select(g => $"{g.Key}: {g.Count()}");
        }

        void hunCompsCount(object sender, RoutedEventArgs e) {
            MessageBox.Show($"Magyar versenyzők száma: {versenyzok.Count(v => v.Nemzetiseg == "HUN")}");
        }
    }
}

MAINWINDOW.XAML ----------------------------------------------------------------->>>

<Window x:Class="TourGUI.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:TourGUI"
        mc:Ignorable="d"
        Title="MainWindow"
        Height="450" 
        Width="800"
        >

    <Grid>

        <Grid.ColumnDefinitions>

            <ColumnDefinition 
                Width="*"
                />

            <ColumnDefinition 
                Width="300"
                />

        </Grid.ColumnDefinitions>

        <DataGrid 
            Grid.Column="0"
            Name="compDisplay"
            SelectionMode="Single"
            SelectionChanged="selectionChanged"
            AutoGenerateColumns="True"
            />

        <StackPanel
            Grid.Column="1"
            >
            <TextBlock
                Name="resultsText"
                Text="5. szakasz eredménye:"
                FontSize="16"
                />

            <Button
                Content="Csapatonkénti versenyzők száma"
                Click="compsCount"
                />

            <ListBox
                Name="teamList"
                Height="150"
                />

            <Button
                Content="Magyar versenyzők száma"
                Click="hunCompsCount"
                />

        </StackPanel>
    </Grid>
</Window>
