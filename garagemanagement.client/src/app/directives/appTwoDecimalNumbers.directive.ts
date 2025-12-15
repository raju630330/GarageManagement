import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appTwoDecimalNumbers]',
  standalone: false,
})
export class TwoDecimalNumbersDirective {
  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent) {
    const input = event.target as HTMLInputElement;
    const char = event.key;

    // digits 0-9
    if (/^[0-9]$/.test(char)) {
      // prevent typing more than 2 decimals
      if (input.value.includes('.')) {
        const decimalPart = input.value.split('.')[1];
        if (decimalPart.length >= 2 && input.selectionStart! > input.value.indexOf('.')) {
          event.preventDefault();
        }
      }
      return;
    }

    // allow only one dot
    if (char === '.' && !input.value.includes('.')) return;

    event.preventDefault();
  }
}
