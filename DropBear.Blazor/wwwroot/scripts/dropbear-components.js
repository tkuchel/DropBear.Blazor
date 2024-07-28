window.DropBearSnackbar = (function () {
  const removalQueue = [];
  let isProcessingRemoval = false;

  function processRemovalQueue() {
    if (isProcessingRemoval || removalQueue.length === 0) return;

    isProcessingRemoval = true;
    const snackbarId = removalQueue.shift();

    const snackbar = document.querySelector(`#${CSS.escape(snackbarId)}`);
    if (snackbar) {
      snackbar.style.animation = 'slideOutAndShrink 0.3s ease-out forwards';
      snackbar.addEventListener('animationend', () => {
        if (snackbar.parentNode) {
          snackbar.parentNode.removeChild(snackbar);
        } else {
          snackbar.remove();
        }
        isProcessingRemoval = false;
        processRemovalQueue();
      }, {once: true});
    } else {
      console.warn(`Snackbar with id ${snackbarId} not found for removal`);
      isProcessingRemoval = false;
      processRemovalQueue();
    }
  }

  return {
    startProgress(snackbarId, duration) {
      const progressBar = document.querySelector(`#${CSS.escape(snackbarId)} .snackbar-progress`);
      console.info('startProgress', snackbarId, duration);

      if (progressBar) {
        progressBar.style.transition = 'none';
        progressBar.style.width = '100%';
        progressBar.style.backgroundColor = getComputedStyle(progressBar).getPropertyValue('color');

        setTimeout(() => {
          progressBar.style.transition = `width ${duration}ms linear`;
          progressBar.style.width = '0%';
        }, 10);
      } else {
        console.error('Progress bar not found');
      }
    },

    hideSnackbar(snackbarId) {
      removalQueue.push(snackbarId);
      processRemovalQueue();
    },

    disposeSnackbar(snackbarId) {
      this.hideSnackbar(snackbarId);
    }
  };
})();
