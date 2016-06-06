using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shannan.DoingWell.UserControls
{
    public partial class DataPager : UserControl
    {
        #region Constants
        private const string ElementRoot = "PART_Root";
        private const string ElementFirstButton = "PART_FirstButton";
        private const string ElementLastButton = "PART_LastButton";
        private const string ElementPreviousButton = "PART_PreviousButton";
        private const string ElementNextButton = "PART_NextButton";
        #endregion Constants

        #region Data
        private Button _firstButton;
        private Button _lastButton;
        private Button _previousButton;
        private Button _nextButton;
        #endregion Data

        public DataPager()
        {
            InitializeComponent();

            this.Loaded += DataPager_Loaded;

        }

        #region Private Properties
        internal Button FirstButton
        {
            get { return this._firstButton; }
        }
        internal Button LastButton
        {
            get { return this._lastButton; }
        }
        internal Button PreviousButton
        {
            get { return this._previousButton; }
        }
        internal Button NextButton
        {
            get { return this._nextButton; }
        }
        #endregion Private Properties

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this._firstButton != null)
            {
                this._firstButton.Click -= new RoutedEventHandler(FirstButton_Click);
            }

            this._firstButton = GetTemplateChild(ElementFirstButton) as Button;

            if (this._firstButton != null)
            {
                if (this._firstButton.Content == null)
                {
                    this._firstButton.Content = "首页";
                }
                this._firstButton.Click += new RoutedEventHandler(FirstButton_Click);
            }



        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DataPager_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

    }
}
