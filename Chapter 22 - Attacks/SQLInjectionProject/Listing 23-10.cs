@{
       var untrustedInput = "<\"testing123\">";
   }

   <div
       id="theDataToBeInjected"
       data-untrustedinput="@untrustedInput" />

   <script>
     var theDataToBeInjected = document.getElementById("theDataToBeInjected");

     //** for all the clients
     var clientWithUntrustedInput =
         theDataToBeInjected.getAttribute("data-untrustedinput");

     //** for clients that support HTML 5
     var clientWithUntrustedInputHtml5 =
         theDataToBeInjected.dataset.untrustedinput;

     document.write(clientWithUntrustedInput);
     document.write("<br />")
     document.write(clientWithUntrustedInputHtml5);
   </script>
