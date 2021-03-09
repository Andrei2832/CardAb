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
using System.Windows.Shapes;

namespace CardAb
{
    /// <summary>
    /// Логика взаимодействия для Zaiav.xaml
    /// </summary>
    
    
    public partial class Zaiav : Window
    {
        Entities1 context = new Entities1();

        List<String> stat = new List<string>();
        List<String> OrgAtt = new List<string>();
        List<String> YesNo = new List<string>();

        Pasport pasport;
        Abiturient abiturient;
        Attestat attestat;
        Declaration declaration;
        public Zaiav()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Data.User == 0)
            {
                Otpravit.Visibility = Visibility.Visible;
                ClearBut.Visibility = Visibility.Visible;
            }
            else if(Data.User == 1)
            {
                Delete.Visibility = Visibility.Visible;
                change.Visibility = Visibility.Visible;
                ClearBut.Visibility = Visibility.Visible;
                LabelSearch.Visibility = Visibility.Visible;
                LabelSearch2.Visibility = Visibility.Visible;
                Search.Visibility = Visibility.Visible;
            }

            stat.AddRange(context.Status.Select(i => i.Наименование_социального_статуса));
            OrgAtt.AddRange(context.Organization.Select(i => i.Кем_выдан));
            YesNo.Add("Нет");
            YesNo.Add("Да");

            status.ItemsSource = stat;
            AttOrg.ItemsSource = OrgAtt;
            AttOrig.ItemsSource = YesNo;
            PaspOrig.ItemsSource = YesNo;
            Photo.ItemsSource = YesNo;
            Med.ItemsSource = YesNo;
            OurHouse.ItemsSource = YesNo;

        }


        private void Otpravit_Click(object sender, RoutedEventArgs e)
        {
            Pasport pasp = context.Pasport.Add(new Pasport()
            {
                Серия_паспорта = Convert.ToInt32(passportSer.Text),
                Номер_паспорта = Convert.ToInt32(passportNum.Text),
                Дата_выдачи = Convert.ToDateTime(DatePassport.Text),
                Кем_выдан = PassportOrg.Text.ToString(),
                pKPP = pKPP.Text.ToString(),
                Адресс_проживания = adressProz.Text.ToString(),
                Адрес_прописки = adressProp.Text.ToString(),
                Дата_рождения = Convert.ToDateTime(dateRod.Text),
                Место_рождения = PlaceRod.Text.ToString()
            });

            int OrgId = context.Organization.Where(i => i.Кем_выдан == AttOrg.Text.ToString()).Select(j => j.id).FirstOrDefault();

            Attestat Att = context.Attestat.Add(new Attestat()
            {
                Серия_аттестата = Convert.ToInt32(AttSer.Text),
                Номер_аттестата = AttNum.Text.ToString(),
                Дата_выдачи = Convert.ToDateTime(DatePasp.Text),
                Кем_выдан = OrgId,
                Средний_балл = Convert.ToDouble(AVG.Text),

            });

            Declaration dec = context.Declaration.Add(new Declaration()
            {
                Оригинал_атестата = AttOrig.SelectedIndex,
                Оригинал_паспорта = PaspOrig.SelectedIndex,
                Фото = Photo.SelectedIndex,
                Медицинская_справка = Med.SelectedIndex,
                Общежитие = OurHouse.SelectedIndex,
            });

            int StatID = context.Status.Where(i => i.Наименование_социального_статуса == status.Text.ToString()).Select(j => j.id).FirstOrDefault();

            Abiturient Adit = context.Abiturient.Add( new Abiturient()
            {
                Фамилия = sname.Text.ToString(),
                Имя = name.Text.ToString(),
                Отчество = fname.Text.ToString(),
                Телефон = phone.Text.ToString(),
                Социальный_статус = StatID,
                Id_Паспорта = pasp.id,
                Id_Заявления = dec.id,
                Id_Аттестата = Att.id,
            });
            context.SaveChanges();
            MessageBox.Show("Отправлено!");
            clearBox();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Search.Text != null)
            {
                int searchBox = Convert.ToInt32(Search.Text);

                pasport = context.Pasport.Where(i => i.Номер_паспорта == searchBox).FirstOrDefault();

                if (pasport != null)
                {
                    abiturient = context.Abiturient.Where(i => i.Id_Паспорта == pasport.id).FirstOrDefault();
                    attestat = context.Attestat.Where(i => i.id == abiturient.Id_Аттестата).FirstOrDefault();
                    declaration = context.Declaration.Where(i => i.id == abiturient.Id_Заявления).FirstOrDefault();

                    sname.Text = abiturient.Фамилия.ToString();
                    name.Text = abiturient.Имя.ToString();
                    fname.Text = abiturient.Отчество.ToString();
                    phone.Text = abiturient.Телефон.ToString();
                    status.SelectedIndex = abiturient.Социальный_статус - 2;

                    passportSer.Text = pasport.Серия_паспорта.ToString();
                    passportNum.Text = pasport.Номер_паспорта.ToString();
                    DatePassport.Text = pasport.Дата_выдачи.ToString();
                    PassportOrg.Text = pasport.Кем_выдан.ToString();
                    pKPP.Text = pasport.pKPP.ToString();
                    adressProz.Text = pasport.Адресс_проживания.ToString();
                    adressProp.Text = pasport.Адрес_прописки.ToString();
                    dateRod.Text = pasport.Дата_рождения.ToString();
                    PlaceRod.Text = pasport.Место_рождения.ToString();

                    AttSer.Text = attestat.Серия_аттестата.ToString();
                    AttNum.Text = attestat.Номер_аттестата.ToString();
                    DatePasp.Text = attestat.Дата_выдачи.ToString();
                    AttOrg.SelectedIndex = attestat.Кем_выдан;
                    AVG.Text = attestat.Средний_балл.ToString();

                    AttOrig.SelectedIndex = declaration.Оригинал_атестата;
                    PaspOrig.SelectedIndex = declaration.Оригинал_паспорта;
                    Photo.SelectedIndex = declaration.Фото;
                    Med.SelectedIndex = declaration.Медицинская_справка;
                    OurHouse.SelectedIndex = declaration.Общежитие;
                }
            }
            
            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Удалить пользователя?","", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                context.Pasport.Remove(pasport);
                context.Abiturient.Remove(abiturient);
                context.Attestat.Remove(attestat);
                context.Declaration.Remove(declaration);
                context.SaveChanges();

                MessageBox.Show("Пользователь удалён!");
                clearBox();
            }
            
            
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Изменить данные?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int StatID = context.Status.Where(i => i.Наименование_социального_статуса == status.Text.ToString()).Select(j => j.id).FirstOrDefault();

                abiturient.Фамилия = sname.Text.ToString();
                abiturient.Имя = name.Text.ToString();
                abiturient.Отчество = fname.Text.ToString();
                abiturient.Телефон = phone.Text.ToString();
                abiturient.Социальный_статус = StatID;

                pasport.Серия_паспорта = Convert.ToInt32(passportSer.Text);
                pasport.Номер_паспорта = Convert.ToInt32(passportNum.Text);
                pasport.Дата_выдачи = Convert.ToDateTime(DatePassport.Text);
                pasport.Кем_выдан = PassportOrg.Text.ToString();
                pasport.pKPP = pKPP.Text.ToString();
                pasport.Адресс_проживания = adressProz.Text.ToString();
                pasport.Адрес_прописки = adressProp.Text.ToString();
                pasport.Дата_рождения = Convert.ToDateTime(dateRod.Text);
                pasport.Место_рождения = PlaceRod.Text.ToString();

                attestat.Серия_аттестата = Convert.ToInt32(AttSer.Text);
                attestat.Номер_аттестата = AttNum.Text.ToString();
                attestat.Дата_выдачи = Convert.ToDateTime(DatePasp.Text);
                attestat.Кем_выдан = AttOrg.SelectedIndex;
                attestat.Средний_балл = Convert.ToDouble(AVG.Text);

                declaration.Оригинал_атестата = AttOrig.SelectedIndex;
                declaration.Оригинал_паспорта = PaspOrig.SelectedIndex;
                declaration.Фото = Photo.SelectedIndex;
                declaration.Медицинская_справка = Med.SelectedIndex;
                declaration.Общежитие = OurHouse.SelectedIndex;

                context.SaveChanges();

                MessageBox.Show("Данные изменены!");
                clearBox();
            }
        }

        private void sname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(pasport != null)
            {
                //if(sname.Text != abiturient.Фамилия.ToString() ||
                //name.Text != abiturient.Имя.ToString() ||
                //fname.Text != abiturient.Отчество.ToString() ||
                //phone.Text != abiturient.Телефон.ToString() ||
                //status.SelectedIndex != abiturient.Социальный_статус - 2 ||

                //passportSer.Text != pasport.Серия_паспорта.ToString() ||
                //passportNum.Text != pasport.Номер_паспорта.ToString() ||
                //DatePassport.Text != pasport.Дата_выдачи.ToString() ||
                //PassportOrg.Text != pasport.Кем_выдан.ToString() ||
                //pKPP.Text != pasport.pKPP.ToString() ||
                //adressProz.Text != pasport.Адресс_проживания.ToString() ||
                //adressProp.Text != pasport.Адрес_прописки.ToString() ||
                //dateRod.Text != pasport.Дата_рождения.ToString() ||
                //PlaceRod.Text != pasport.Место_рождения.ToString() ||

                //AttSer.Text != attestat.Серия_аттестата.ToString() ||
                //AttNum.Text != attestat.Номер_аттестата.ToString() ||
                //DatePasp.Text != attestat.Дата_выдачи.ToString() ||
                //AttOrg.SelectedIndex != attestat.Кем_выдан ||
                //AVG.Text != attestat.Средний_балл.ToString() ||

                //AttOrig.SelectedIndex != declaration.Оригинал_атестата ||
                //PaspOrig.SelectedIndex != declaration.Оригинал_паспорта ||
                //Photo.SelectedIndex != declaration.Фото ||
                //Med.SelectedIndex != declaration.Медицинская_справка ||
                //OurHouse.SelectedIndex != declaration.Общежитие)
                //{
                //    Search.IsEnabled = false;
                //}
            }
        }

        public void clearBox()
        {
            sname.Clear();
            name.Clear();
            fname.Clear();
            phone.Clear();
            status.SelectedIndex = -1;

            passportSer.Clear();
            passportNum.Clear();
            DatePassport.Text = null;
            PassportOrg.Clear();
            pKPP.Clear();
            adressProz.Clear();
            adressProp.Clear();
            dateRod.Text = null;
            PlaceRod.Clear();

            AttSer.Clear();
            AttNum.Clear();
            DatePasp.Text = null;
            AttOrg.SelectedIndex = -1;
            AVG.Clear();

            AttOrig.SelectedIndex = -1;
            PaspOrig.SelectedIndex = -1;
            Photo.SelectedIndex = -1;
            Med.SelectedIndex = -1;
            OurHouse.SelectedIndex = -1;

        }

        private void ClearBut_Click(object sender, RoutedEventArgs e)
        {
            clearBox();
        }
    }
}