   M S B A M L   `   `   `  ����H  DOnlineBanking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:OnlineBankingOnlineBanking  5   	        OnlineBanking.DialogWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:OnlineBanking     5   	     � Title$  DialogWindow��5   	   $	��700��6   $	��640��6+    � ShowInTaskbar .   � Topmost .  � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   
������ � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ProjectTheme.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $	��.1* �6   6+   �� 5      $��3* �6   6*   �� 5      6   �� 5      $	��.1* �6   6+   	 � ColumnDefinitions	 5   
   �� 5      $	��.1* �6   6-   �� 5      6!   �� 5       $	��.1* �6   6-   .+�5"   
   �� -   5        $��ContinueButton��5"   m   $��Continue��6   $��1��6$   $��2��64   $
��Auto��6A   $��Center=�6O   $��Center=�6�   5#      �� 5$   
   -   5        $��	textBlock��5$   g   $��1��6   $��1��6$   $
��Auto��61   $
��Auto��6?   $
��Left=�6L   $��[instructions go here]��6x   $	��Top=�6�   $��30(�6�   $��WrapWithOverflow=�6�   5%      5&      