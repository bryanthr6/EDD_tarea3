using System;
using Gtk;
using System.Collections.Generic;

public class Nodo
{
    public int ID;
    public string Repuesto;
    public string Detalles;
    public Nodo Siguiente;
}

public class ListaCircular
{
    private Nodo cabeza;
    
    public void AgregarRepuesto(int id, string repuesto, string detalles)
    {
        Nodo nuevo = new Nodo { ID = id, Repuesto = repuesto, Detalles = detalles };
        if (cabeza == null)
        {
            cabeza = nuevo;
            cabeza.Siguiente = cabeza;
        }
        else
        {
            Nodo temp = cabeza;
            while (temp.Siguiente != cabeza)
            {
                temp = temp.Siguiente;
            }
            temp.Siguiente = nuevo;
            nuevo.Siguiente = cabeza;
        }
    }
    
    public Nodo BuscarRepuesto(int id)
    {
        if (cabeza == null) return null;
        Nodo temp = cabeza;
        do
        {
            if (temp.ID == id)
                return temp;
            temp = temp.Siguiente;
        } while (temp != cabeza);
        return null;
    }
}

public class MainWindow : Window
{
    private ListaCircular lista = new ListaCircular();
    private Entry idEntry, repuestoEntry, detallesEntry;
    private Label resultadoLabel;

    public MainWindow() : base("Lista Circular de Repuestos")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox();

        idEntry = new Entry { PlaceholderText = "ID" };
        repuestoEntry = new Entry { PlaceholderText = "Nombre del repuesto" };
        detallesEntry = new Entry { PlaceholderText = "Detalles" };
        Button agregarBtn = new Button("Agregar Repuesto");
        Button buscarBtn = new Button("Buscar Repuesto por ID");
        resultadoLabel = new Label("");

        agregarBtn.Clicked += AgregarRepuesto;
        buscarBtn.Clicked += BuscarRepuesto;

        vbox.PackStart(idEntry, false, false, 5);
        vbox.PackStart(repuestoEntry, false, false, 5);
        vbox.PackStart(detallesEntry, false, false, 5);
        vbox.PackStart(agregarBtn, false, false, 5);
        vbox.PackStart(buscarBtn, false, false, 5);
        vbox.PackStart(resultadoLabel, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private void AgregarRepuesto(object sender, EventArgs e)
    {
        if (int.TryParse(idEntry.Text, out int id))
        {
            lista.AgregarRepuesto(id, repuestoEntry.Text, detallesEntry.Text);
            resultadoLabel.Text = "Repuesto agregado con éxito";
        }
        else
        {
            resultadoLabel.Text = "ID inválido";
        }
    }

    private void BuscarRepuesto(object sender, EventArgs e)
    {
        if (int.TryParse(idEntry.Text, out int id))
        {
            Nodo encontrado = lista.BuscarRepuesto(id);
            if (encontrado != null)
                resultadoLabel.Text = $"Repuesto: {encontrado.Repuesto}, Detalles: {encontrado.Detalles}";
            else
                resultadoLabel.Text = "Repuesto no encontrado";
        }
        else
        {
            resultadoLabel.Text = "ID inválido";
        }
    }
}

class Program
{
    public static void Main()
    {
        Application.Init();
        new MainWindow();
        Application.Run();
    }
}