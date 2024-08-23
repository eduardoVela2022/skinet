import { Component, inject } from '@angular/core';
import { ShopService } from '../../../core/service/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule,
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss',
})
export class FiltersDialogComponent {
  // API service
  shopService = inject(ShopService);
  // Dialog dependencies
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  // The selected brands and types of the dialog (modal)
  selectedBrands: string[] = this.data.selectedBrands;
  selectedTypes: string[] = this.data.selectedTypes;

  // Result
  applyFilters() {
    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedTypes: this.selectedTypes,
    });
  }
}
