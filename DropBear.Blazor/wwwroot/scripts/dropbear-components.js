// DropBearSnackbar
window.DropBearSnackbar = (function () {
  const removalQueue = [];
  let isProcessingRemoval = false;

  function processRemovalQueue() {
    if (isProcessingRemoval || removalQueue.length === 0) return;

    isProcessingRemoval = true;
    const snackbarId = removalQueue.shift();

    const snackbar = document.getElementById(snackbarId);
    if (snackbar) {
      console.log(`Removing snackbar ${snackbarId}`);
      snackbar.style.animation = 'slideOutDown 0.3s ease-out forwards';
      snackbar.addEventListener('animationend', () => {
        snackbar.remove();
        isProcessingRemoval = false;
        processRemovalQueue();
      }, {once: true});
    } else {
      console.warn(`Snackbar ${snackbarId} not found for removal`);
      isProcessingRemoval = false;
      processRemovalQueue();
    }
  }

  return {
    startProgress(snackbarId, duration) {
      console.log(`Starting progress for snackbar ${snackbarId} with duration ${duration}`);

      // Ensure the snackbar is visible
      this.showSnackbar(snackbarId);

      // Verify the snackbar after a short delay
      setTimeout(() => this.verifySnackbar(snackbarId), 100);

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

      // Schedule snackbar removal
      this.scheduleSnackbarRemoval(snackbarId, duration);
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
      console.log(`Hiding snackbar ${snackbarId}`);
      const snackbar = document.getElementById(snackbarId);
      if (snackbar) {
        snackbar.style.animation = 'slideOutDown 0.3s ease-out forwards';
        snackbar.addEventListener('animationend', () => {
          console.log(`Animation ended for snackbar ${snackbarId}, removing from DOM`);
          snackbar.remove();
        }, {once: true});
      } else {
        console.error(`Snackbar ${snackbarId} not found when trying to hide it`);
      }
    },

    scheduleSnackbarRemoval(snackbarId, duration) {
      console.log(`Scheduling removal of snackbar ${snackbarId} after ${duration}ms`);
      setTimeout(() => {
        console.log(`Time's up for snackbar ${snackbarId}, initiating removal`);
        this.hideSnackbar(snackbarId);
      }, duration);
    },

    disposeSnackbar(snackbarId) {
      console.log(`Disposing snackbar ${snackbarId}`);
      this.hideSnackbar(snackbarId);
    },

    verifySnackbar(snackbarId) {
      const snackbar = document.getElementById(snackbarId);
      if (snackbar) {
        console.log('Snackbar found in DOM:', snackbar);
        console.log('Snackbar visibility:', window.getComputedStyle(snackbar).visibility);
        console.log('Snackbar display:', window.getComputedStyle(snackbar).display);
        console.log('Snackbar opacity:', window.getComputedStyle(snackbar).opacity);
      } else {
        console.error('Snackbar not found in DOM');
      }
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
