using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Shannan.DoingWell.UserControls
{
    public partial class Pager : UserControl
    {
        #region Public Events
        public event RoutedEventHandler CurrentPageChanged;
        #endregion Public Events

        public Pager()
        {
            InitializeComponent();
            this.Loaded += Pager_Loaded;
        }

        private void Pager_Loaded(object sender, RoutedEventArgs e)
        {
            PART_FirstButton.Click += FirstButton_Click;
            PART_LastButton.Click += LastButton_Click;
            PART_PreviousButton.Click += PreviousButton_Click;
            PART_NextButton.Click += NextButton_Click;
            this.RefreshLayout();
        }

        #region Public Properties

        #region Total
        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(Pager), new PropertyMetadata(0, TotalChanged));

        private static void TotalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Pager).CalPageCount();
        }
        #endregion Total

        #region PageSize
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(Pager), new PropertyMetadata(0, PageSizeChanged));

        private static void PageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Pager).CalPageCount();
        }
        #endregion PageSize

        #region PageCount
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            private set { SetValue(PageCountProperty, value); }
        }
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(Pager), new PropertyMetadata(0, PageCountChanged));

        private static void PageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Pager).CurrentPage = 1;
            (d as Pager).RefreshLayout();
        }
        #endregion PageCount

        #region CurrentPage
        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(Pager), new PropertyMetadata(0));
        #endregion CurrentPage

        #region ShowNumber
        public int ShowNumber
        {
            get { return (int)GetValue(ShowNumberProperty); }
            set { SetValue(ShowNumberProperty, value); }
        }

        public static readonly DependencyProperty ShowNumberProperty =
            DependencyProperty.Register("ShowNumber", typeof(int), typeof(Pager), new PropertyMetadata(0));
        #endregion ShowNumber

        #endregion Public Properties

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            this.RefreshLayout();
            this.RaiseEvent();
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = PageCount;
            this.RefreshLayout();
            this.RaiseEvent();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage -= 1;
            CurrentPage = Math.Max(CurrentPage, 1);
            this.RefreshLayout();
            this.RaiseEvent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
            CurrentPage = Math.Min(CurrentPage, PageCount);
            this.RefreshLayout();
            this.RaiseEvent();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = int.Parse((sender as Control).Tag.ToString());
            this.RefreshLayout();
            this.RaiseEvent();
        }

        private void CalPageCount()
        {
            if (PageSize <= 0)
            {
                PageCount = 0;
            }
            else
            {
                PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Total) / Convert.ToDouble(PageSize)));
            }
        }

        private void RefreshLayout()
        {
            CurrentPage = Math.Min(CurrentPage, PageCount);
            CurrentPage = Math.Max(CurrentPage, 1);

            PART_Total.Text = Total.ToString();
            PART_PageCount.Text = PageCount.ToString();

            PART_FirstButton.IsEnabled = PART_PreviousButton.IsEnabled = (CurrentPage != 1);
            PART_LastButton.IsEnabled = PART_NextButton.IsEnabled = (CurrentPage != PageCount);

            int finalShowNumber = Math.Min(ShowNumber, PageCount);

            int firstDisplayNumber = Math.Max(1, CurrentPage - Convert.ToInt32(Math.Floor(Convert.ToDouble(finalShowNumber) / 2)));
            firstDisplayNumber = Math.Min(firstDisplayNumber, (PageCount + 1 - finalShowNumber));

            ObservableCollection<Object> numPagersSource = new ObservableCollection<Object>();
            for (int i = 0; i < finalShowNumber; i++)
            {
                numPagersSource.Add(new
                {
                    Number = (firstDisplayNumber + i).ToString(),
                    Style = (firstDisplayNumber + i == CurrentPage) ? Resources["SNButtonCurrent"] : Resources["SNButtonNormal"]
                });
            }
            numberPagers.ItemsSource = numPagersSource;

            if (Total <= 0)
            {
                PART_FirstButton.IsEnabled = PART_PreviousButton.IsEnabled = false;
                PART_LastButton.IsEnabled = PART_NextButton.IsEnabled = false;
            }
        }

        private void RaiseEvent()
        {
            if (this.CurrentPageChanged != null)
            {
                this.CurrentPageChanged(this, new RoutedEventArgs());
            }
        }
    }
}
