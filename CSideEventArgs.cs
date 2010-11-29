using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// An event arguments class for C/Side events
   /// </summary>
   public class CSideEventArgs : EventArgs
   {
		#region Properties (5) 

      /// <summary>
      /// Gets or sets the current company.
      /// </summary>
      /// <value>The current company.</value>
      public string CurrentCompany { get; set; }

      /// <summary>
      /// Gets or sets the current database.
      /// </summary>
      /// <value>The current database.</value>
      public string CurrentDatabase { get; set; }

      /// <summary>
      /// Gets or sets the form.
      /// </summary>
      /// <value>The form.</value>
      public Form Form { get; set; }

      /// <summary>
      /// Gets or sets the previous company.
      /// </summary>
      /// <value>The previous company.</value>
      public string PreviousCompany { get; set; }

      /// <summary>
      /// Gets or sets the previous database.
      /// </summary>
      /// <value>The previous database.</value>
      public string PreviousDatabase { get; set; }

		#endregion Properties 

		#region Constructors/Deconstructors (3) 

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs(ChangeEventType type, string previous, string current)
      {
         switch (type)
         {
            case ChangeEventType.Database:
               PreviousDatabase = previous;
               CurrentDatabase = current;
               break;
            case ChangeEventType.Company:
               PreviousCompany = previous;
               CurrentCompany = current;
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs(Form form)
      {
         Form = form;
      }

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs()
      {

      }

		#endregion Constructors/Deconstructors 
   }
}
