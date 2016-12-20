using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpEdit.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Fields

        private const string defaultText = "Welcome to UWP Edit. The text editor for UWP apps.";

        private const string loreumTest = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam convallis ex ut ante accumsan rutrum. Morbi in volutpat libero. Vestibulum non ligula quis lacus consequat luctus. Maecenas molestie ipsum quis ipsum sagittis lobortis. Fusce lorem libero, convallis vitae metus ut, volutpat suscipit mauris. Nunc faucibus sem sit amet tincidunt rhoncus. Aliquam blandit ex et libero mollis, et blandit nisi faucibus. Donec sit amet pretium lacus. Sed rutrum pulvinar urna id varius. Aenean sodales justo vel nisi congue, in sollicitudin magna euismod. Integer iaculis ultricies dignissim. Mauris vitae orci tempus, mattis ligula ac, viverra tellus. Donec id nunc eget purus dignissim rhoncus. Nullam nec turpis volutpat, dignissim eros eu, elementum ligula.

Vivamus accumsan ex quis eros condimentum sagittis nec non orci. Aliquam erat volutpat. Ut a dapibus urna, eget cursus mi. Proin id fermentum lectus. Mauris sed neque vel tortor imperdiet elementum eget eget libero. Morbi vel mollis mauris, ac vestibulum quam. Suspendisse tincidunt iaculis porta. Nulla consequat tellus vel est vestibulum, sed feugiat nisi porta. Nulla posuere porta lacus, id sodales lectus eleifend eu. Fusce tincidunt pulvinar felis, in euismod purus placerat sed. Quisque sed justo aliquam, commodo erat in, mattis odio. Nunc non orci vel ligula ornare maximus. Pellentesque imperdiet tortor quis lacus ultricies, eleifend ullamcorper eros lobortis.

Nulla iaculis erat erat, quis interdum augue elementum sit amet. Cras eleifend, tellus id laoreet sodales, turpis velit commodo felis, at condimentum magna ante ac nulla. Sed tempor ipsum at purus semper, nec scelerisque diam auctor. Integer efficitur posuere interdum. Donec consequat arcu malesuada malesuada aliquet. Sed in tellus sollicitudin, sodales diam ac, luctus metus. Maecenas finibus posuere ornare. Sed ac ipsum ultricies, porta sapien in, rutrum lectus. Maecenas iaculis laoreet diam quis pellentesque. Pellentesque dictum elit a nulla venenatis, a efficitur tortor accumsan. Cras vulputate finibus rutrum. Donec laoreet, dolor sit amet scelerisque imperdiet, leo risus varius magna, ac pellentesque elit leo nec augue. Suspendisse imperdiet ligula in ante faucibus rhoncus. Nunc in fermentum nisi. In sed scelerisque elit.

Duis pulvinar dolor augue, eu elementum est posuere sed. Vestibulum gravida, nibh id consequat placerat, mi sem tristique nisi, ullamcorper lacinia tellus lacus a mauris. Sed placerat, tortor eu varius efficitur, lorem ex consectetur augue, sit amet lobortis orci sapien at dui. Suspendisse euismod posuere est, sit amet tempor ligula fermentum id. Sed ultricies erat in lectus aliquet feugiat. Sed id cursus metus. Proin ligula dolor, gravida ac placerat non, tincidunt vitae est. In eu volutpat dui, nec sodales nisl. Aenean malesuada fermentum enim at venenatis. Donec non varius eros. Pellentesque vestibulum ipsum nec dolor aliquet, eget viverra libero dictum. Aenean id egestas mauris, nec accumsan urna. Maecenas semper pharetra enim, sit amet interdum arcu consequat at.

Phasellus sit amet justo accumsan, euismod enim et, tristique massa. Pellentesque ac massa urna. Aenean non ligula in metus commodo porttitor id ut lorem. Nullam et ex eget odio porta hendrerit sit amet quis nisi. Maecenas sit amet diam a ante consequat finibus. Phasellus risus purus, rhoncus quis tincidunt in, ultricies sed eros. Integer egestas arcu lectus, ac tincidunt tortor ultricies eget. Cras sagittis orci risus, id rutrum lorem tristique ut. Donec ipsum quam, vehicula quis laoreet eu, laoreet at sem. Cras sit amet vestibulum justo. Aliquam erat volutpat. Curabitur et volutpat arcu. Donec et tincidunt ante, et iaculis justo. Curabitur finibus mauris nec semper aliquam. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.

Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Quisque quis laoreet neque, ut semper dolor. Nullam id mattis quam. Sed porta ipsum nulla, eu egestas nunc posuere vitae. Curabitur elit diam, molestie nec enim eget, ornare lacinia magna. Aenean at felis leo. Proin condimentum, nisi ut pulvinar egestas, odio sapien vestibulum nisl, at hendrerit sapien ante pharetra tellus. Suspendisse est purus, tincidunt eget suscipit at, varius at nisi.

Integer tellus dolor, semper non orci nec, efficitur finibus magna. Aenean ut dictum metus, sit amet semper est. Morbi congue ultricies enim, ac pellentesque elit scelerisque sed. Vestibulum vulputate fringilla ante, sit amet blandit metus suscipit eu. Etiam laoreet nibh id nisl aliquet, sed sollicitudin velit pellentesque. Aenean dapibus porttitor nisl. Curabitur posuere laoreet mauris quis tempus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Quisque fermentum eu lorem et rhoncus. Maecenas augue nisi, placerat laoreet dolor eget, auctor suscipit lorem. Nam non risus commodo, suscipit massa non, egestas mauris. Donec lorem nunc, porttitor ac dolor sed, posuere mollis nisi. Fusce sit amet fermentum lacus.

Suspendisse ultrices nulla eget augue vulputate, nec consectetur quam congue. Nam blandit tristique aliquam. Nulla diam ipsum, ullamcorper sit amet ante et, eleifend mattis ligula. Quisque consequat lobortis nisi. Vivamus at purus imperdiet, finibus ligula a, semper lorem. Ut finibus pharetra ante, sit amet congue metus tincidunt ut. Duis aliquam leo ac ipsum imperdiet vulputate. Suspendisse sit amet lacinia odio. Suspendisse pellentesque felis sit amet scelerisque facilisis. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec fringilla dapibus venenatis. Nam eu euismod tortor. Nullam ac nisl est.

Aenean finibus metus lectus, at pharetra ipsum malesuada id. Nunc id est aliquet leo volutpat interdum. Vivamus ullamcorper rhoncus vestibulum. Ut bibendum lectus dolor, vitae porta mauris molestie nec. Aliquam tempor nibh sed ultrices euismod. Fusce sit amet odio accumsan, placerat ipsum vel, molestie sem. Praesent eu vulputate est, egestas placerat est. Sed bibendum mattis quam, vel sodales lacus viverra sit amet.

Integer eleifend sem a hendrerit rutrum. Duis dapibus volutpat pellentesque. Nunc convallis tincidunt augue, malesuada dictum lectus tempus et. Praesent efficitur ligula eget facilisis gravida. Vivamus consequat metus fringilla sapien tristique ornare. Sed vehicula, magna eget facilisis pellentesque, nisl dui malesuada nisl, eu pretium tortor ex nec arcu. Suspendisse potenti. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec consectetur urna quis odio cursus viverra. Suspendisse purus enim, vulputate ac erat id, auctor posuere metus. Suspendisse potenti. Interdum et malesuada fames ac ante ipsum primis in faucibus. Vivamus eu vestibulum turpis. Curabitur vestibulum et tellus nec elementum.";

        private Dictionary<int, FontWeight> _fontWeights = new Dictionary<int, FontWeight>()
        {
            {0, FontWeights.Black },
            {1, FontWeights.Bold },
            {2, FontWeights.ExtraBlack },
            {3, FontWeights.ExtraBold },
            {4, FontWeights.ExtraLight },
            {5, FontWeights.Light },
            {6, FontWeights.Medium },
            {7, FontWeights.Normal },
            {8, FontWeights.SemiBold },
            {9, FontWeights.SemiLight },
            {10, FontWeights.Thin },
        };

        private int _selectedFontWeight = 7;

        #endregion Private Fields

        #region Public Constructors

        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        public static DependencyProperty TextProperty { get; } = DependencyProperty.Register(nameof(Text), typeof(string), typeof(MainPage), new PropertyMetadata(loreumTest));
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        #endregion Public Properties

        #region Private Methods

        private void DecreaseFontSizeButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize--;
        }

        private void IncreaseFontSizeButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize++;
        }

        private void SelectRangeButton_Click(object sender, RoutedEventArgs e)
        {
            var length = Math.Min(5, textEditor.Text.Length);
            textEditor.Focus(FocusState.Programmatic);
            textEditor.Select(0, length);
        }

        private void ToggleFontFamilyButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.FontFamily == Application.Current.Resources["ContentControlThemeFontFamily"])
            {
                textEditor.FontFamily = new FontFamily("Consolas");
            }
            else if (textEditor.FontFamily.Source == "Consolas")
            {
                textEditor.FontFamily = new FontFamily("Arial");
            }
            else
            {
                textEditor.FontFamily = (FontFamily)Application.Current.Resources["ContentControlThemeFontFamily"];
            }
        }

        private void ToggleFontStretchButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)textEditor.FontStretch < 9)
            {
                textEditor.FontStretch = (FontStretch)((int)textEditor.FontStretch + 1);
            }
            else
            {
                textEditor.FontStretch = (FontStretch)1;
            }
        }

        private void ToggleFontStyleButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)textEditor.FontStyle < 2)
            {
                textEditor.FontStyle = (FontStyle)((int)textEditor.FontStyle + 1);
            }
            else
            {
                textEditor.FontStyle = (FontStyle)0;
            }
        }

        private void ToggleFontWeightButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFontWeight < 9)
            {
                _selectedFontWeight++;
            }
            else
            {
                _selectedFontWeight = 0;
            }
            textEditor.FontWeight = _fontWeights[_selectedFontWeight];
        }

        private void ToggleForegroundButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.Foreground == Application.Current.Resources["SystemControlForegroundBaseHighBrush"])
            {
                textEditor.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                textEditor.Foreground = (SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"];
            }
        }

        private void ToggleHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.Header == null)
            {
                textEditor.Header = "UwpEdit.TextEditor";
            }
            else
            {
                textEditor.Header = null;
            }
        }

        private void ToggleHeaderTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.HeaderTemplate == null)
            {
                var dataTemplateXaml = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Grid><TextBlock Text=\"{Binding}\" Foreground=\"Red\" FontSize=\"15\"/></Grid></DataTemplate>";
                var dataTemplate = XamlReader.Load(dataTemplateXaml) as DataTemplate;
                textEditor.HeaderTemplate = dataTemplate;
            }
            else
            {
                textEditor.HeaderTemplate = null;
            }
        }

        private void TogglePlaceholderTextButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textEditor.PlaceholderText))
            {
                textEditor.PlaceholderText = "This is some placeholder text.";
            }
            else
            {
                textEditor.PlaceholderText = string.Empty;
            }
        }

        private void ToggleSelectionColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.SelectionHighlightColor == Application.Current.Resources["SystemControlHighlightAccentBrush"])
            {
                textEditor.SelectionHighlightColor = new SolidColorBrush(Colors.Green);
            }
            else
            {
                textEditor.SelectionHighlightColor = (SolidColorBrush)Application.Current.Resources["SystemControlHighlightAccentBrush"];
            }
        }

        private void ToggleTextAlignmentButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)textEditor.TextAlignment < 4)
            {
                textEditor.TextAlignment = (TextAlignment)((int)textEditor.TextAlignment + 1);
            }
            else
            {
                textEditor.TextAlignment = (TextAlignment)0;
            }
        }

        #endregion Private Methods
    }
}
