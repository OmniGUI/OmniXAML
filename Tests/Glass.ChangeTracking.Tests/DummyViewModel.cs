namespace Glass.ChangeTracking.Tests
{
    public class DummyViewModel : ViewModel
    {
        private int number;
        private DummyViewModel child;
        private string text;
        private MyStruct myStruct;

        public int Number
        {
            get { return number; }
            set
            {
                if (value == number)
                {
                    return;
                }
                number = value;
                OnPropertyChanged();
            }
        }

        public DummyViewModel Child
        {
            get { return child; }
            set
            {
                if (Equals(value, child))
                {
                    return;
                }
                child = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value == text)
                {
                    return;
                }
                text = value;
                OnPropertyChanged();
            }
        }

        public MyStruct MyStruct
        {
            get { return myStruct; }
            set
            {
                if (value.Equals(myStruct))
                {
                    return;
                }
                myStruct = value;
                OnPropertyChanged();
            }
        }
    }
}