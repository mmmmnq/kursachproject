   M S B A M L   `   `   `  ����D  @kursovaya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:kursovaya.ViewModelskursovaya.ViewModels  5   	       kursovaya.Views.UserForm   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1vm"clr-namespace:kursovaya.ViewModels     5   	     � Title$  Туры��5   	   $	��500��6   $	��800��6#   ���#1e1e1e � DataContext 5      (   "kursovaya.ViewModels.TourViewModel  5   
   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $
��Auto=�6   .��5      �� ��6   �� *Tours+��5      �� 5      .R�5      �� ���#444444$��1q�67    �� CornerRadius$ 8��6K   $��10q�6\   ���#333333._�5      � .+�5   "   �� $��UniformToFill=�6E   $	��200��6]   ��6(   �� *	ImagePath+6]   �� 5   "   $
��Bold"�6B   $��16(�6T   
����������	#AA000000$
��Left=�5   ,   $��Bottom=�6G    �� Padding$ 5q�6b   $��
10,0,0,159q�6n   ��5   ,   �� *Name+5   n   �� 5   "   $��14(�6_   
����������	#99000000$
��Left=�5    ,   $��Bottom=�6G   $��625,0,0,125q�6b   $ 5q�6w   $	��115��6�   ��5   ,   �� *Price+ �� StringFormat$ Цена: {0:C}��5    �   �� 5#   "    �� Orientation$ 
Horizontal=�6-   $��Bottom=�6F   $
��Left=�6a   $��
531,0,0,10q�6|   .+�5$   &   �� $ ��Забронировать��6-   $	��100��6E   ���#007ACC
�������$��5q�6y   ��6�   �� *DataContext.BookCommand+ �� RelativeSource ��  �� AncestorType$ WindowL���6�   �� �� 5&   &   $��Подробнее��6-   $	��100��5'   	   ���#444444
�������$��5q�5*   	   ��5+   	   �� *DataContext.DetailsCommand+ �� $ WindowL���5,   	   �� 5/   #   50      51      52      54      55      56      57      