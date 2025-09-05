using Gum.DataTypes;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderingLibrary.Graphics;
using MonoGameGum;

namespace YnamarEditors
{
    internal class MenuManager
    {
        private GumProjectSave _gumProject;
        private GraphicalUiElement _currentScreen;

        public MenuManager(GumProjectSave gumProject)
        {
            _gumProject = gumProject;
        }

        /// <summary>
        /// Loads a Gum screen by name and makes it active.
        /// </summary>
        public GraphicalUiElement LoadScreen(string screenName)
        {
            // Remove old screen if exists
            _currentScreen?.RemoveFromRoot();

            // Create new screen
            var screenRuntime = _gumProject.Screens
                .First(s => s.Name == screenName)
                .ToGraphicalUiElement();

            screenRuntime.AddToRoot();
            _currentScreen = screenRuntime;

            return _currentScreen;
        }

        /// <summary>
        /// Gets the currently active Gum screen.
        /// </summary>
        public GraphicalUiElement GetCurrentScreen() => _currentScreen;
    }
}
