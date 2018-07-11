namespace MatchingsInGraphs
{
    using System;
    using System.Linq.Expressions;
    using System.Windows;

    using MatchingsCore.Graph;
    using MatchingsCore.Serializers;

    using Microsoft.Win32;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The graph.
        /// </summary>
        private Graph graph;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        ///     The load file button click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void LoadFileButtonClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { InitialDirectory = Environment.CurrentDirectory };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var serializer = new AdjacencyListSerializer();
                this.graph = serializer.Deserialize(filePath);
            }
            else
            {
                throw new Exception("Error while showing openFileDialog");
            }
        }
    }
}