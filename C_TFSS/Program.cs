using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;


namespace Actualizar_TFS
{
    public class Program
    {
        static void Main(string[] args)
        {
            // URL de TFS
            string tfsUrl = "http://desktop-gqopia9:8080/tfs/DefaultCollection";

            // Iteración actual y nueva iteración
            string currentIteration = "ITyruiz\\2023\\Mayo_Q1";
            string newIteration = "ITyruiz\\2023\\Mayo_Q2";

            // Conexión a TFS
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(tfsUrl));
            tfs.Authenticate();

            // Obtener el servicio de trabajo de TFS
            WorkItemStore workItemStore = tfs.GetService<WorkItemStore>();

            // Consulta para seleccionar las tareas que cumplen con el criterio
            string query = $"SELECT [System.Id], [System.IterationPath] " +
                $"FROM WorkItems " +
                $"WHERE [System.WorkItemType] = 'Task' " +
                $"AND [System.IterationPath] = '{currentIteration}' " +
                $"AND [System.State] = 'Active' " +
                $"AND [System.NodeName] Contains 'Starlabs'";

            // Ejecutar la consulta
            WorkItemCollection workItems = workItemStore.Query(query);

            // Modificar el campo Iteration de las tareas seleccionadas
            foreach (WorkItem workItem in workItems)
            {
                workItem.Open();
                workItem.Fields["System.IterationPath"].Value = newIteration;
                workItem.Save();
                Console.WriteLine("Se actualizó la " + workItem.Fields["System.WorkItemType"].Value + " " + workItem.Fields["System.Id"].Value + ". Nuevo valor de Iteration: " + workItem.Fields["System.IterationPath"].Value);

            }

            // Cerrar la conexión a TFS
            tfs.Dispose();
            Console.WriteLine(" ");
            Console.WriteLine("Tareas actualizadas con éxito.");
            Console.ReadLine();
        }
    }
}