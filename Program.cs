using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace ListarEnderecosIP
{
    class Monitor
    {
        public static List<string> listaIPsConectadosNessePC()
        {
            // Pega as conexoes ativas
            IPGlobalProperties relacionamentosRede = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] conexoesAbertasCom = relacionamentosRede.GetActiveTcpConnections();

            // Cria uma lista para armazenar os endereços IP remotos únicos
            List<string> espioesIPs = new List<string>();

            Console.WriteLine("\nEstes IPs estão se metando conosco:\n");

            // Percorre todas as conexões ativas e adiciona os endereços IP remotos únicos à lista
            foreach (TcpConnectionInformation desteHost in conexoesAbertasCom)
            {
                string espiaoIP = desteHost.RemoteEndPoint.Address.ToString();
                espioesIPs.Add(espiaoIP);
                Console.WriteLine(espiaoIP);
            }
            
            // Para efeitos futoros retorno os ips para caso um dia eu queria fazer alguma coisa com eles
            return espioesIPs;
        }

        public static void bloqueiaIP(string ip, string porta)
        {
            if(!ip.Equals("") && !porta.Equals(""))
               System.Diagnostics.Process.Start("netsh","advfirewall firewall add rule name=\"javinha"+ip+porta+"\"" + " dir=out protocol=TCP remoteip="+ip+" remoteport="+porta+" action=block");
        }

        public static void prompt()
        {
            Console.WriteLine("\nOpcoes: bloquear | nada | listar | sair");
        }

        public static int loopingInteracao()
        {
            string opcao = "";
            while (true)
            {
                List<string> espioes = listaIPsConectadosNessePC();
                prompt();
                opcao = Console.ReadLine();
                if (opcao.Equals("bloquear"))
                {
                    foreach (string espiao in espioes)
                    {
                        bloqueiaIP(espiao, "443");
                    }
                }
                if (opcao.Equals("sair"))
                {
                    return 1;
                }

            }

        }


        static void Main(string[] args)
        {
            loopingInteracao();
        }
    }
}
