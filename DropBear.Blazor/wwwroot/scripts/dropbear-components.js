// DropBearSnackbar
window.DropBearSnackbar = (function () {
  const snackbars = new Map();

  function removeSnackbar(snackbarId) {
    const snackbar = document.getElementById(snackbarId);
    if (snackbar) {
      snackbar.addEventListener('animationend', () => {
        snackbar.remove();
        snackbars.delete(snackbarId);
      }, {once: true});
      snackbar.style.animation = 'slideOutDown 0.3s ease-out forwards';
    } else {
      console.warn(`Snackbar ${snackbarId} not found for removal`);
      snackbars.delete(snackbarId);
    }
  }

  return {
    startProgress(snackbarId, duration) {
      console.log(`Starting progress for snackbar ${snackbarId} with duration ${duration}`);

      if (snackbars.has(snackbarId)) {
        console.warn(`Snackbar ${snackbarId} already exists. Removing old instance.`);
        this.hideSnackbar(snackbarId);
      }

      this.showSnackbar(snackbarId);

      const progressBar = document.querySelector(`#${CSS.escape(snackbarId)} .snackbar-progress`);
      if (progressBar) {
        console.log('Progress bar found, setting up animation');
        progressBar.style.transition = 'none';
        progressBar.style.width = '100%';
        progressBar.style.backgroundColor = getComputedStyle(progressBar).getPropertyValue('color');

        setTimeout(() => {
          console.log('Starting progress bar animation');
          progressBar.style.transition = `width ${duration}ms linear`;
          progressBar.style.width = '0%';
        }, 10);
      } else {
        console.error('Progress bar not found');
      }

      snackbars.set(snackbarId, setTimeout(() => this.hideSnackbar(snackbarId), duration));
    },

    showSnackbar(snackbarId) {
      console.log(`Showing snackbar ${snackbarId}`);
      const snackbar = document.getElementById(snackbarId);
      if (snackbar) {
        snackbar.style.display = 'flex';
        snackbar.style.opacity = '1';
        console.log(`Snackbar ${snackbarId} display set to flex and opacity set to 1`);
      } else {
        console.error(`Snackbar ${snackbarId} not found when trying to show it`);
      }
    },

    hideSnackbar(snackbarId) {
      if (snackbars.has(snackbarId)) {
        console.log(`Hiding snackbar ${snackbarId}`);
        clearTimeout(snackbars.get(snackbarId));
        snackbars.delete(snackbarId);
        removeSnackbar(snackbarId);
      } else {
        console.warn(`Snackbar ${snackbarId} not found in active snackbars`);
      }
    },

    disposeSnackbar(snackbarId) {
      console.log(`Disposing snackbar ${snackbarId}`);
      this.hideSnackbar(snackbarId);
    }
  };
})();


// DropBearModal
window.DropBearModal = (function () {
  return {
    initialize() {
      document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape') {
          DotNet.invokeMethodAsync('DropBear.Blazor', 'CloseModalOnEscape');
        }
      });
    },
    focusFirstInput(modalId) {
      const modal = document.getElementById(modalId);
      if (modal) {
        const input = modal.querySelector('input, select, textarea');
        if (input) {
          input.focus();
        }
      }
    },
    updateModalTheme(modalId, themeClass) {
      const modal = document.getElementById(modalId);
      if (modal) {
        modal.classList.remove("theme-dark", "theme-light");
        modal.classList.add(themeClass);
      }
    },
    hideModal(modalId) {
      const modal = document.getElementById(modalId);
      if (modal) {
        modal.classList.remove("active");
      }
    }
  };
})();

// DropBearFileUploader
window.DropBearFileUploader = (function () {
  let droppedFiles = [];

  function handleDrop(e) {
    e.preventDefault();
    e.stopPropagation();

    droppedFiles = [];

    try {
      if (e.dataTransfer.items) {
        for (let i = 0; i < e.dataTransfer.items.length; i++) {
          if (e.dataTransfer.items[i].kind === 'file') {
            const file = e.dataTransfer.items[i].getAsFile();
            droppedFiles.push({
              name: file.name,
              size: file.size,
              type: file.type
            });
          }
        }
      } else {
        for (let i = 0; i < e.dataTransfer.files.length; i++) {
          const file = e.dataTransfer.files[i];
          droppedFiles.push({
            name: file.name,
            size: file.size,
            type: file.type
          });
        }
      }
    } catch (error) {
      console.error('Error handling dropped files:', error);
    }
  }

  function init() {
    document.addEventListener('drop', function (e) {
      if (e.target.closest('.file-upload-dropzone')) {
        handleDrop(e);
      }
    });

    document.addEventListener('dragover', function (e) {
      if (e.target.closest('.file-upload-dropzone')) {
        e.preventDefault();
        e.stopPropagation();
      }
    });
  }

  // Initialize when the DOM is ready
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }

  return {
    getDroppedFiles() {
      const files = droppedFiles;
      droppedFiles = [];
      return files;
    },

    clearDroppedFiles() {
      droppedFiles = [];
    }
  };
})();

// Utility function for file download
window.downloadFileFromStream = (fileName, byteArray) => {
  const blob = new Blob([byteArray], {type: "application/octet-stream"});
  const url = window.URL.createObjectURL(blob);
  const a = document.createElement("a");
  document.body.appendChild(a);
  a.style = "display: none";
  a.href = url;
  a.download = fileName;
  a.click();
  window.URL.revokeObjectURL(url);
};

// DropBearContextMenu
window.DropBearContextMenu = (function () {
  class ContextMenu {
    constructor(element, dotNetReference) {
      this.element = element;
      this.dotNetReference = dotNetReference;
      this.initialize();
    }

    initialize() {
      this.element.addEventListener('contextmenu', this.handleContextMenu.bind(this));
      document.addEventListener('click', this.handleDocumentClick.bind(this));
    }

    handleContextMenu(e) {
      e.preventDefault();
      this.show(e.clientX, e.clientY);
    }

    handleDocumentClick() {
      this.dotNetReference.invokeMethodAsync('Hide');
    }

    show(x, y) {
      this.dotNetReference.invokeMethodAsync('Show', x, y);
    }

    dispose() {
      this.element.removeEventListener('contextmenu', this.handleContextMenu);
      document.removeEventListener('click', this.handleDocumentClick);
    }
  }

  const menuInstances = new Map();

  return {
    initialize(elementId, dotNetReference) {
      const element = document.getElementById(elementId);
      if (!element) {
        console.error(`Element with id '${elementId}' not found.`);
        return;
      }

      if (menuInstances.has(elementId)) {
        console.warn(`Context menu for element '${elementId}' already initialized. Disposing old instance.`);
        this.dispose(elementId);
      }

      const menuInstance = new ContextMenu(element, dotNetReference);
      menuInstances.set(elementId, menuInstance);
    },

    show(elementId, x, y) {
      const menuInstance = menuInstances.get(elementId);
      if (menuInstance) {
        menuInstance.show(x, y);
      } else {
        console.error(`No context menu instance found for element '${elementId}'.`);
      }
    },

    dispose(elementId) {
      const menuInstance = menuInstances.get(elementId);
      if (menuInstance) {
        menuInstance.dispose();
        menuInstances.delete(elementId);
      }
    },

    disposeAll() {
      menuInstances.forEach((instance, elementId) => instance.dispose());
      menuInstances.clear();
    }
  };
})();
