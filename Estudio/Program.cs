using System;
using System.Collections.Generic; // Importé esto para poder usar Listas

namespace ProyectoCuestionarioPOO
{
    // Esta es la clase abstracta que funciona como mi clase base.
    // La hice abstracta para que no se puedan crear objetos "Tarjeta" vacíos o genéricos.
    public abstract class Tarjeta
    {
        // Pongo los atributos como protected para aplicar el encapsulamiento. 
        // Así solo las clases hijas pueden acceder a ellos, pero desde el Main están protegidos.
        protected string pregunta;
        protected string respuestaCorrecta;

        // Este es el constructor de mi clase base para inicializar los valores.
        public Tarjeta(string pregunta, string respuestaCorrecta)
        {
            this.pregunta = pregunta;
            this.respuestaCorrecta = respuestaCorrecta;
        }

        // Imprime el título y la pregunta. 
        // Lo hice virtual por si alguna clase hija necesita hacerle override para imprimir más cosas.
        public virtual void MostrarPregunta()
        {
            Console.WriteLine("--- PREGUNTA ---");
            Console.WriteLine(pregunta);
        }

        // Aquí declaro el método abstracto. Esto obliga a mis clases hijas a aplicar polimorfismo 
        // y crear su propia forma específica de evaluar la respuesta del usuario.
        public abstract bool Evaluar(string respuestaUsuario);
    }


    // --- PRIMERA CLASE HIJA ---
    // Aquí aplico herencia. TarjetaAbierta hereda de la clase base Tarjeta.
    public class TarjetaAbierta : Tarjeta
    {
        // En el constructor uso la palabra 'base' para mandarle los parámetros al constructor del padre.
        public TarjetaAbierta(string pregunta, string respuestaCorrecta) : base(pregunta, respuestaCorrecta)
        {
        }

        // Polimorfismo: Aquí le hago override al método abstracto con mi propia lógica.
        public override bool Evaluar(string respuestaUsuario)
        {
            // Uso .Trim() y .ToLower() para limpiar lo que escribe el usuario, 
            // así si pone un espacio extra o mayúsculas, no le cuenta como error.
            if (respuestaUsuario.Trim().ToLower() == respuestaCorrecta.ToLower())
            {
                Console.WriteLine("Correcto");
                return true;
            }
            else
            {
                Console.WriteLine("Incorrecto. La respuesta correcta era: " + respuestaCorrecta);
                return false;
            }
        }
    }


    // --- SEGUNDA CLASE HIJA ---
    // Esta clase también hereda de Tarjeta, pero se comporta distinto.
    public class TarjetaOpcionMultiple : Tarjeta
    {
        // Atributo encapsulado y exclusivo de esta clase hija.
        private string[] opciones;

        // El constructor ahora recibe también un arreglo con las opciones (A, B, C...).
        public TarjetaOpcionMultiple(string pregunta, string respuestaCorrecta, string[] opciones) : base(pregunta, respuestaCorrecta)
        {
            this.opciones = opciones;
        }

        // Polimorfismo: Le hago override a MostrarPregunta para imprimir las opciones además del texto.
        public override void MostrarPregunta()
        {
            base.MostrarPregunta(); // Primero llamo al método del padre

            // Luego uso un ciclo for para recorrer y mostrar el arreglo de opciones.
            for (int i = 0; i < opciones.Length; i++)
            {
                Console.WriteLine(opciones[i]);
            }
        }

        // Polimorfismo: La lógica para evaluar aquí es distinta porque solo busco que coincida la letra.
        public override bool Evaluar(string respuestaUsuario)
        {
            if (respuestaUsuario.Trim().ToLower() == respuestaCorrecta.ToLower())
            {
                Console.WriteLine("¡Excelente!");
                return true;
            }
            else
            {
                Console.WriteLine("Incorrecto. La opción correcta era: " + respuestaCorrecta);
                return false;
            }
        }
    }


    // --- CLASE GESTORA ---
    // Esta clase funciona como mi colección principal de datos.
    public class BancoPreguntas
    {
        // Encapsulamiento de la lista de tarjetas y el contador de puntos.
        private List<Tarjeta> tarjetas;
        private int puntaje;

        public BancoPreguntas()
        {
            // Instancio la lista vacía al crear el banco de preguntas.
            tarjetas = new List<Tarjeta>();
            puntaje = 0;
        }

        // Método para agregar objetos a mi lista.
        public void AgregarTarjeta(Tarjeta nuevaTarjeta)
        {
            tarjetas.Add(nuevaTarjeta);
        }

        // Este es el método que controla todo el flujo en la consola.
        public void IniciarPrueba()
        {
            Console.WriteLine("=== BIENVENIDO AL CUESTIONARIO DE ESTUDIO ===");

            // Recorro mi lista de objetos.
            foreach (Tarjeta t in tarjetas)
            {
                // Llamada polimórfica: El programa decide automáticamente cuál MostrarPregunta() ejecutar.
                t.MostrarPregunta();

                Console.Write("Tu respuesta: ");
                string respuesta = Console.ReadLine();

                // Segunda llamada polimórfica para evaluar si sumo un punto o no.
                if (t.Evaluar(respuesta))
                {
                    puntaje++;
                }
            }

            Console.WriteLine("=== RESULTADOS FINALES ===");
            Console.WriteLine("Puntaje total: " + puntaje + " de " + tarjetas.Count);
        }
    }


    // --- PROGRAMA PRINCIPAL ---
    class Program
    {
        static void Main(string[] args)
        {
            // Instancio el objeto principal que manejará el juego.
            BancoPreguntas miCuestionario = new BancoPreguntas();

            // Creo mi primera tarjeta usando la clase de pregunta abierta.
            TarjetaAbierta pregunta1 = new TarjetaAbierta("¿Cuál es el lenguaje de programación que usamos en esta clase?", "C#");

            // Para las tarjetas de opción múltiple, primero armo mi arreglo de texto.
            string[] opcionesPregunta2 = { "A) private", "B) public", "C) protected" };
            TarjetaOpcionMultiple pregunta2 = new TarjetaOpcionMultiple("¿Qué modificador de acceso permite a las clases hijas ver un atributo?", "C", opcionesPregunta2);

            string[] opcionesPregunta3 = { "A) Herencia", "B) Polimorfismo", "C) Encapsulamiento" };
            TarjetaOpcionMultiple pregunta3 = new TarjetaOpcionMultiple("¿Qué concepto nos permite sobrescribir un método de la clase padre?", "B", opcionesPregunta3);

            // Agrego los tres objetos instanciados a mi colección principal.
            miCuestionario.AgregarTarjeta(pregunta1);
            miCuestionario.AgregarTarjeta(pregunta2);
            miCuestionario.AgregarTarjeta(pregunta3);

            // Llamo al método que arranca el bucle del cuestionario.
            miCuestionario.IniciarPrueba();

            // Pongo este ReadKey para evitar que la ventana negra se cierre inmediatamente al terminar.
            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}