using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WYSIWYGTextEditor;

namespace Oversteer.BlazorComponents.Championship
{
    public partial class Rules
    {


        [Parameter]
        public Models.Racing.Championship Championship { get; set; } = new Models.Racing.Championship();

        TextEditor DetailsEditor;
        TextEditor RulesEditor;


    }
}
