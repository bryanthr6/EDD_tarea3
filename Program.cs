using System;
using Gtk;
using System.Runtime.InteropServices;

public unsafe class Nodo
{
    public int ID;
    public string Repuesto;
    public string Detalles;
    public Nodo* Siguiente;
}

public unsafe class ListaCircular
{
    private Nodo* cabeza = null;

    public void Agregar(int id, string repuesto, string detalles)
    {
        Nodo* nuevo = (Nodo*)Marshal.AllocHGlobal(sizeof(Nodo));
        nuevo->ID = id;
        nuevo->Repuesto = repuesto;
        nuevo->Detalles = detalles;
        nuevo->Siguiente = null;

        if (cabeza == null)
        {
            cabeza = nuevo;
            cabeza->Siguiente = cabeza;
        }
        else
        {
            Nodo* temp = cabeza;
            while (temp->Siguiente != cabeza)
                temp = temp->Siguiente;
            temp->Siguiente = nuevo;
            nuevo->Siguiente = cabeza;
        }
    }

    public Nodo* Buscar(int id)
    {
        if (cabeza == null) return null;
        Nodo* temp = cabeza;
        do
        {
            if (temp->ID == id)
                return temp;
            temp = temp->Siguiente;
        } while (temp != cabeza);
        return null;
    }
}

public class AplicacionGTK : Window
{
    private Entry entryID, entryRepuesto, entryDetalles;
    private TextView output;
    private ListaCircular lista = new ListaCircular();

    public AplicacionGTK() : base("Lista Circular - GTK")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox();

        entryID = new Entry { PlaceholderText = "ID" };
        entryRepuesto = new Entry { PlaceholderText = "Repuesto" };
        entryDetalles = new Entry { PlaceholderText = "Detalles" };
        Button btnAgregar = new Button("Agregar");
        Button btnBuscar = new Button("Buscar");
        output = new TextView { Editable = false };

        btnAgregar.Clicked += OnAgregarClicked;
        btnBuscar.Clicked += OnBuscarClicked;

        vbox.PackStart(entryID, false, false, 5);
        vbox.PackStart(entryRepuesto, false, false, 5);
        vbox.PackStart(entryDetalles, false, false, 5);
        vbox.PackStart(btnAgregar, false, false, 5);
        vbox.PackStart(btnBuscar, false, false, 5);
        vbox.PackStart(output, true, true, 5);

        Add(vbox);
        ShowAll();
    }

    private void OnAgregarClicked(object sender, EventArgs e)
    {
        if (int.TryParse(entryID.Text, out int id))
        {
            lista.Agregar(id, entryRepuesto.Text, entryDetalles.Text);
            output.Buffer.Text += $"Agregado: {id}, {entryRepuesto.Text}, {entryDetalles.Text}\n";
        }
        else
        {
            output.Buffer.Text += "ID inválido.\n";
        }
    }

    private void OnBuscarClicked(object sender, EventArgs e)
    {
        if (int.TryParse(entryID.Text, out int id))
        {
            unsafe
            {
                Nodo* encontrado = lista.Buscar(id);
                if (encontrado != null)
                {
                    output.Buffer.Text += $"Encontrado: {encontrado->ID}, {encontrado->Repuesto}, {encontrado->Detalles}\n";
                }
                else
                {
                    output.Buffer.Text += "No encontrado.\n";
                }
            }
        }
        else
        {
            output.Buffer.Text += "ID inválido.\n";
        }
    }

    public static void Main()
    {
        Application.Init();
        new AplicacionGTK();
        Application.Run();
    }
}
