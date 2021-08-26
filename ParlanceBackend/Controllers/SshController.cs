using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParlanceBackend.Authentication;
using ParlanceBackend.Models;

namespace ParlanceBackend.Controllers
{
    [Route("api/ssh")]
    [ApiController]
    [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName, Roles = ProjectsAuthorizationHandler.ModifySshKeysPermission)]
    public class SshController : ControllerBase
    {
        string SshFile(string file)
        {
            var sshFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh-parlance");
            if (!Directory.Exists(sshFolder)) Directory.CreateDirectory(sshFolder);
            return Path.Join(sshFolder, file);
        }
        
        [HttpGet("publicKey")]
        public async Task<ActionResult<SshKeyData>> AcquirePublicKey()
        {
            //Send the contents of the .ssh/id_rsa.pub file

            try
            {
                return new SshKeyData()
                {
                    SshKeyContents = await System.IO.File.ReadAllTextAsync(SshFile("id_rsa.pub"))
                };
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPost("publicKey")]
        public async Task<ActionResult<SshKeyData>> GeneratePublicKey()
        {
            //Send the contents of the .ssh/id_rsa.pub file
            if (System.IO.File.Exists(SshFile("id_rsa.pub")))
            {
                return Conflict();
            }
            
            using var keygenProcess = new Process
            {
                StartInfo =
                {
                    FileName = "ssh-keygen",
                    Arguments = $"-t rsa -f {SshFile("id_rsa")} -q -P \"\" -C Parlance"
                }
            };

            keygenProcess.Start();
            await keygenProcess.WaitForExitAsync();

            if (keygenProcess.ExitCode != 0)
            {
                throw new Exception("ssh-keygen unable to create key");
            }

            return await AcquirePublicKey();
        }
        
        [HttpDelete("publicKey")]
        public async Task<ActionResult<SshKeyData>> DeleteSshKey()
        {
            //Delete the keys used for ssh
            try
            {
                System.IO.File.Delete(SshFile("id_rsa.pub"));
                System.IO.File.Delete(SshFile("id_rsa"));
                return NoContent();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPost("hostKeys")]
        public async Task<ActionResult> AcquireSshKey(AcquireSshHostKeyData data)
        {
            //Use ssh-keyscan to scan keys and add them to the .ssh/known_hosts file

            using var keyscanProcess = new Process
            {
                StartInfo =
                {
                    FileName = "ssh-keyscan",
                    
                    //TODO: Could this be dangerous???
                    Arguments = $"{data.Host}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                },
            };

            keyscanProcess.Start();
            await keyscanProcess.WaitForExitAsync();

            if (keyscanProcess.ExitCode != 0)
            {
                throw new Exception("ssh-keyscan unable to scan");
            }

            await using var stream = System.IO.File.AppendText(SshFile("known_hosts"));
            await stream.WriteAsync(await keyscanProcess.StandardOutput.ReadToEndAsync());
            stream.Close();
            
            return NoContent();
        }
        
        
        [HttpGet("hostKeys")]
        public async Task<ActionResult<string[]>> AcquireHostKeys()
        {
            //Send the contents of the .ssh/known_hosts file
            try
            {
                return await System.IO.File.ReadAllLinesAsync(SshFile("known_hosts"));
            }
            catch (FileNotFoundException)
            {
                return Array.Empty<string>();
            }
        }
        
        
        [HttpDelete("hostKeys/{index}")]
        public async Task<ActionResult> DeleteHostKey(int index)
        {
            try
            {
                //Delete a host key
                var keys = (await System.IO.File.ReadAllLinesAsync(SshFile("known_hosts"))).ToList();
                keys.RemoveAt(index);
                await System.IO.File.WriteAllLinesAsync(SshFile("known_hosts"), keys.ToArray());

                return NoContent();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        }
    }
}