using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
    public partial class MainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translatedNumber;
        public MainPage()
        {
            InitializeComponent();
            this.Padding = new Thickness(20, 20, 20, 20);
            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(new Label { 
                Text = "Enter a Phoneword:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            panel.Children.Add(phoneNumberText = new Entry { 
                Text = "1-855-XAMARIN",
            });

            panel.Children.Add(translateButton = new Button {
                Text = "Translate",
            });

            panel.Children.Add(callButton = new Button
            {
                Text = "call",
                IsEnabled = false,
            });

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }
        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = Phoneword.PhonewordTranslator.ToNumber(enteredNumber);

            if(!string.IsNullOrEmpty(translatedNumber))
            {
                //todo
                callButton.IsEnabled = true;
                callButton.Text = "Call" + translatedNumber;
            }
            else
            {
                //todo
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }

        async void OnCall(object sender, System.EventArgs e)
        {
            if (await this.DisplayAlert ("Dial a Number", "Would you like to call"  + translatedNumber + "?", "Yes", "No"))
            {
                //TODO: dial the phone
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid", "Ok");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "Ok");
                }
                catch (Exception)
                {
                    //Other error has occurred.
                    await DisplayAlert("Unable to dial", "Phone dial failed.", "Ok");
                }
            }
        }
    }
}
