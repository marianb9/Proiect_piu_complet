[README.md](https://github.com/user-attachments/files/28194124/README.md)
# Sistem Informatic pentru Gestionarea Inspecțiilor Tehnice Periodice Auto

Aplicație desktop dezvoltată în **C# / WPF (.NET 10)** în cadrul cursului de Proiectare Interfețe Utilizator.

---

## Entități

| Entitate | Câmpuri |
|---|---|
| **Proprietar** | Nume, CNP, Telefon |
| **Mașină** | Marcă, Model, An fabricație, Nr. înmatriculare, Culoare, Dotări |
| **Inspecție ITP** | Data, Rezultat (Admis/Respins), Valabil până la, Defecțiuni |
| **Inspector** | Nume, Serie insignă |

---

## Funcționalități

- Adăugare / editare / ștergere mașini
- Înregistrare inspecție nouă pentru o mașină
- Istoric ITP pentru fiecare mașină
- Calcul automat dată expirare ITP (2 ani de la inspecție)
- Căutare mașini după nr. înmatriculare, marcă, model sau proprietar
- Avertizare dacă ITP-ul este expirat
- Avertizare dacă mașina a fost respinsă de 2 ori consecutiv
- Validare la înregistrarea a două inspecții în aceeași zi pentru aceeași mașină

---

## Rapoarte

- Număr mașini admise / respinse într-o perioadă
- Cele mai frecvente defecte înregistrate
- Grafic cu rata de promovare lunară
- Listă mașini cu ITP expirat

---

## Tehnologii utilizate

- C# / .NET 10
- WPF (Windows Presentation Foundation)
- XAML pentru interfața grafică
- Stocare date în fișiere text
- Pattern arhitectural MVVM

---

## Structura proiectului

```
Proiect_piu_complet/
├── Modele_PIU/          # Entitati: Masina, Proprietar, InspectieITP, Inspector
├── Manager_PIU/         # Logica de acces la date (fisiere text)
├── Proiect_PIU/         # Aplicatia consola (entry point)
└── ITP_WPF/             # Interfata grafica WPF
    ├── MainWindow        # Fereastra principala cu lista masini
    ├── AdaugaMasinaWindow    # Formular adaugare masina (MVVM + IDataErrorInfo)
    ├── ModificaMasinaWindow  # Modificare masina existenta
    └── GestionareInspectiiWindow  # CRUD complet inspectii ITP
```

---

## Rulare

1. Clonează repository-ul
2. Deschide `Proiect_PIU.slnx` în Visual Studio 2022+
3. Setează `ITP_WPF` ca Startup Project
4. Apasă `F5`
