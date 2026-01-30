# 🔴 PokeReport PDF Generator

![Estat](https://img.shields.io/badge/Estat-Completat-success?style=for-the-badge&logo=github)
![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=for-the-badge&logo=dotnet)
![QuestPDF](https://img.shields.io/badge/Llibreria-QuestPDF-blue?style=for-the-badge)
![WPF](https://img.shields.io/badge/UI-WPF-b02010?style=for-the-badge)

Una aplicació d'escriptori **WPF** dissenyada per generar informes visuals detallats de Pokémon en format PDF utilitzant la potència de **QuestPDF**. 

Aquest projecte replica l'estètica d'una Pokédex moderna, integrant gràfics d'estadístiques, etiquetes de tipus amb colors dinàmics i gestió d'imatges en temps real.

---

## 📋 Descripció del Projecte

Desenvolupat com a part de l'assignatura de **Desenvolupament d'Interfícies**, l'objectiu principal és demostrar la generació programàtica de documents complexos sense utilitzar dissenyadors visuals "drag-and-drop".

L'aplicació es connecta a una base de dades MySQL per obtenir informació detallada (estadístiques base, tipus, noms), descarrega els actius gràfics oficials i compila un informe paginat i professional.

### 🖼️ Previsualització
*(Pots afegir aquí una captura de pantalla de la teva aplicació o del PDF generat)*

---

## ✨ Característiques Principals

### 🎨 Disseny i Maquetació (QuestPDF)
* **Layout de 3 Columnes:** Estructura clara amb Imatge, Informació i Estadístiques.
* **Estètica "Card":** Contenidors amb vores arrodonides (`BorderRadius`) i espaiat cuidat.
* **Gràfics de Barres Simulats:** Visualització de stats (HP, ATK, DEF...) alineades a la base mitjançant codi C#.
* **Colors Dinàmics:** Les etiquetes de tipus (ex: *GRASS*, *POISON*) obtenen el seu color hexadecimal (`#78C850`, etc.) directament de la base de dades.

### ⚙️ Funcionalitats Tècniques
* **Càrrega Asíncrona:** Pantalla de càrrega (`LoadingScreen`) amb barra de progrés real mentre es descarreguen les imatges.
* **Sistema de Memòria Cau:** Les imatges es guarden a la carpeta local `/Images` per evitar descàrregues innecessàries en futures execucions.
* **Visor Integrat:** Utilització de `WebView2` per previsualitzar el PDF generat dins de la mateixa finestra WPF.
* **Entity Framework Core:** Consultes complexes amb `Include` per unir taules de Pokémon, Tipus i Estadístiques Base.

---

## 🛠️ Tecnologies Utilitzades

| Tecnologia | Ús |
| :--- | :--- |
| **C# (.NET 6.0)** | Llenguatge principal i lògica de negoci. |
| **WPF** | Interfície d'usuari (XAML). |
| **QuestPDF** | Motor de generació de PDF (Codi obert). |
| **Entity Framework Core** | ORM per a l'accés a dades. |
| **MySQL / MariaDB** | Base de dades relacional. |
| **WebView2** | Component per visualitzar el PDF a l'app. |

---

## 🚀 Instal·lació i Posada en Marxa

1.  **Clonar el repositori:**
    ```bash
    git clone https://github.com/kuro-spm/DI_P3_PokeReport.git
    ```

2.  **Configurar la Base de Dades:**
    * Importa el fitxer `dump-pokedex-original.sql` (inclòs al projecte) al teu servidor MySQL (XAMPP, Workbench, etc.).
    * Verifica la cadena de connexió (`connection string`) a `AppDbContext.cs` o `InvoiceDocumentDataSource.cs`.

3.  **Restaurar Dependències:**
    Obre la solució a Visual Studio i deixa que es restaurin els paquets NuGet:
    * `QuestPDF`
    * `Microsoft.EntityFrameworkCore`
    * `Pomelo.EntityFrameworkCore.MySql` (o similar)

4.  **Executar:**
    Prem **F5**. La primera vegada veuràs la barra de progrés mentre es descarreguen les imatges dels Pokémon.

---

## 📂 Estructura del Codi

L'arquitectura separa clarament la lògica de la vista de la generació del document:

* `MainWindow.xaml`: Orquestra la UI, la barra de progrés i el visor web.
* `InvoiceDocument.cs`: **Nucli del disseny**. Conté tota la lògica visual de QuestPDF (Capçalera, Files de Pokémon, Gràfics).
* `InvoiceDocumentDataSource.cs`: Servei encarregat de parlar amb la BD i preparar el model.
* `Models/`: Classes POCO (`Pokemon`, `Type`, `BaseStat`) mapejades des de la base de dades.

---

## ✒️ Autor

**Sara Prats Morales**
* [LinkedIn](https://www.linkedin.com/in/sara-prats-morales)
* [GitHub](https://github.com/kuro-spm)

