using System.Security.Cryptography;

// Prueba: calcular el hash de "123" exactamente como lo hace el código
string password = "123";

using var sha = SHA256.Create();
var bytes = System.Text.Encoding.UTF8.GetBytes(password.Trim());
var hash = sha.ComputeHash(bytes);
var hexHash = string.Concat(hash.Select(b => b.ToString("x2")));

Console.WriteLine($"Contraseña: {password}");
Console.WriteLine($"Hash esperado: {hexHash}");
Console.WriteLine($"Longitud: {hexHash.Length}");

// Ahora vamos a verificar lo que está en la BD
