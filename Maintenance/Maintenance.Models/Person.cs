using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.Models
{
    public class Person {
        public int Id { get; set; } // id человека
        public string Surname { get; set; } // фамилия
        public string Name { get; set; } // имя
        public string Patronymic { get; set; }  // отчество
        public string Passport { get; set; }    // паспорт

        // в данном случае используется 1 к 1 потому что клиент может быть только один
        // с такими данными (уникальность по id и уникальность по пасспорту)
        public virtual Client Client { get; set; }    // ссылка на клиента

        // так же будет 1 к 1 с работником потому что такой работник будет абсолютно уникальным
        public virtual Worker Worker { get; set; }    // ссылка на работника
        public virtual ICollection<Car> Cars { get; set; }  // список ссылок на машины которыми может владеть персона

        // конструктор
        public Person() {
            Cars = new HashSet<Car>();
        } // People - конструктор
    } // People
}
