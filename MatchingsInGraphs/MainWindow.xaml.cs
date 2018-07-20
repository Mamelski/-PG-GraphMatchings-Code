namespace MatchingsInGraphs
{
    using System;
    using System.Windows;

    using MatchingsCore;
    using MatchingsCore.GraphRepresentation;
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
        /// The utils.
        /// </summary>
        private Utils utils = new Utils();

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
                var serializer = new AdjacencyListGraphSerializer();
                this.graph = serializer.Deserialize(filePath);
            }
            else
            {
                throw new Exception("Error while showing openFileDialog");
            }
        }

        /// <summary>
        /// The button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var isBigraf = this.utils.ColortGraphAndCheckIfItIsBipartite(ref this.graph);
        }
    }
}