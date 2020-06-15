using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Person : INotifyPropertyChanged
    {
        public int Id { get; set; } // id человека

        // фамилия
        private string _surname;

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            } // set
        } // Surname

        // имя
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            } // set
        } // Name

        // отчество
        private string _patronymic;

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged();
            } // set
        } // Patronymic

        // паспорт
        private string _passport;

        public string Passport
        {
            get => _passport;
            set
            {
                _passport = value;
                OnPropertyChanged();
            } // set
        } // Passport

        // в данном случае используется 1 к 1 потому что клиент может быть только один
        // с такими данными (уникальность по id и уникальность по пасспорту)
        public virtual Client Client { get; set; } // ссылка на клиента

        // так же будет 1 к 1 с работником потому что такой работник будет абсолютно уникальным
        public virtual Worker Worker { get; set; } // ссылка на работника
        public virtual ICollection<Car> Cars { get; set; } // список ссылок на машины которыми может владеть персона

        // конструктор
        public Person()
        {
            Cars = new HashSet<Car>();
        } // People - конструктор

        // -----------------------------------------------------------------------------
        // реализация интерфейса INotifyPropertyChanged - взял из прошлых работ
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    } // People
}
